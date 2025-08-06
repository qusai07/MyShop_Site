using Microsoft.EntityFrameworkCore;
using MyShop_Site.Data;
using MyShop_Site.Models;
using System.Security.Cryptography;
using System.Text;

namespace MyShop_Site.Services
{
    public class UserService
    {
        private readonly MyShopDbContext _context;

        public UserService(MyShopDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsUsernameAvailableAsync(string username)
        {
            return !await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> IsEmailAvailableAsync(string email)
        {
            return !await _context.Users.AnyAsync(u => u.ContactEmail == email);
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            user.PasswordHash = HashPassword(password);
            user.CreatedDate = DateTime.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> RegisterAsync(User user)
        {
            try
            {
                // Check if username or email already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == user.Username || u.ContactEmail == user.ContactEmail);

                if (existingUser != null)
                {
                    return null; // User already exists
                }

                // Hash password (in production, use proper password hashing)
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.CreatedAt = DateTime.UtcNow;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch
            {
                return null;
            }
        }

        // Helper method for password hashing (replace with a strong hashing algorithm in production)
        private string HashPassword(string password)
        {
            // Example using a simple hash, replace with BCrypt or similar for production
            using (var sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Helper method for password verification (replace with a strong hashing algorithm in production)
        private bool VerifyPassword(string providedPassword, string hashedPassword)
        {
            // Example using a simple hash, replace with BCrypt or similar for production
            return HashPassword(providedPassword) == hashedPassword;
        }

        // Helper method to get user by username
        private async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}