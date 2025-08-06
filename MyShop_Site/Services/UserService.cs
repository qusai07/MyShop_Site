
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

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hash;
        }
    }
}
