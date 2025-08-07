using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using MyShop_Site.Repo.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MyShop_Site.Repo.Implementations
{
    public class SecureCookieService : ISecureCookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _dataProtector;
        private readonly ILogger<SecureCookieService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _encryptionKey = "MyShop2024SecureKey123!@#"; // يفضل استخدام Configuration


        public SecureCookieService(IHttpContextAccessor httpContextAccessor,IDataProtectionProvider dataProtectionProvider,ILogger<SecureCookieService> logger,IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _dataProtector = dataProtectionProvider.CreateProtector("MyShop.SecureCookies");
            _logger = logger;
            _configuration = configuration;
        }

        public void SetSecureCookie(string key, string value, TimeSpan? expiry = null, bool essential = false)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null) return;

            
                // Encrypt the value
                var encryptedValue = _dataProtector.Protect(value);

                var options = CreateSecureCookieOptions(expiry, essential);

                httpContext.Response.Cookies.Append(key, encryptedValue, options);

                _logger.LogInformation("Secure cookie set: {CookieKey}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting secure cookie: {CookieKey}", key);
            }
        }
    
        public void SetSecureCookie<T>(string key, T value, TimeSpan? expiry = null, bool essential = false)
        {
            try
            {
                var jsonValue = JsonSerializer.Serialize(value);
                SetSecureCookie(key, jsonValue, expiry, essential);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error serializing and setting secure cookie: {CookieKey}", key);
            }
        }

        public string GetCookie(string key)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null) return null;

                var encryptedValue = httpContext.Request.Cookies[key];
                if (string.IsNullOrEmpty(encryptedValue)) return null;

                // Decrypt the value
                return _dataProtector.Unprotect(encryptedValue);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error reading secure cookie: {CookieKey}", key);
                // Delete corrupted cookie
                DeleteCookie(key);
                return null;
            }
        }

        public T GetCookie<T>(string key) where T : class
        {
            try
            {
                var jsonValue = GetCookie(key);
                if (string.IsNullOrEmpty(jsonValue)) return null;

                return JsonSerializer.Deserialize<T>(jsonValue);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error deserializing secure cookie: {CookieKey}", key);
                DeleteCookie(key);
                return null;
            }
        }

        public void DeleteCookie(string key)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null) return;

                var options = CreateSecureCookieOptions(TimeSpan.FromDays(-1));
                httpContext.Response.Cookies.Append(key, "", options);

                _logger.LogInformation("Secure cookie deleted: {CookieKey}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting secure cookie: {CookieKey}", key);
            }
        }

        public void SetAuthenticationCookie(string userId, bool rememberMe = false)
        {
            var authData = new AuthCookieData
            {
                UserId = userId,
                IssuedAt = DateTime.UtcNow,
                SessionId = Guid.NewGuid().ToString()
            };

            var expiry = rememberMe ? TimeSpan.FromDays(30) : TimeSpan.FromHours(8);
            SetSecureCookie("auth_session", authData, expiry, true);

            // Set CSRF token
            SetSecureCookie("csrf_token", Guid.NewGuid().ToString(), expiry, true);
        }

        public void ClearAuthenticationCookie()
        {
            DeleteCookie("auth_session");
            DeleteCookie("csrf_token");
        }

        public bool ValidateSecureCookie(string key)
        {
            try
            {
                var value = GetCookie(key);
                return !string.IsNullOrEmpty(value);
            }
            catch
            {
                return false;
            }
        }

        private CookieOptions CreateSecureCookieOptions(TimeSpan? expiry = null, bool essential = false)
        {
            var isDevelopment = _configuration.GetValue<bool>("Environment:IsDevelopment");

            return new CookieOptions
            {
                // Security flags
                HttpOnly = true,                    // Prevent XSS attacks
                Secure = !isDevelopment,           // HTTPS only in production
                SameSite = SameSiteMode.Strict,    // CSRF protection

                // Expiration
                Expires = expiry.HasValue ? DateTime.UtcNow.Add(expiry.Value) : null,
                MaxAge = expiry,

                // Path and domain
                Path = "/",
                Domain = null, // Let browser determine

                // GDPR compliance App Needed
                IsEssential = essential
            };
        }

        public void SetJsonCookie<T>(string key, T data, int expireMinutes = 30)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var expireTimeSpan = TimeSpan.FromMinutes(expireMinutes);
                SetSecureCookie(key, json, expireTimeSpan);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"خطأ في حفظ بيانات JSON: {ex.Message}");
            }
        }

        public void SetRememberMeCookie(int userId, string username, int expireDays = 30)
        {
            var rememberData = new
            {
                UserId = userId,
                Username = username,
                CreatedAt = DateTime.UtcNow
            };

            SetJsonCookie("RememberMe", rememberData, expireDays * 24 * 60);
        }

        public (int UserId, string Username)? GetRememberMeData()
        {
            try
            {
                var data = GetCookie<dynamic>("RememberMe");

                if (data != null)
                {
                    var jsonElement = (JsonElement)data;
                    return (
                        jsonElement.GetProperty("UserId").GetInt32(),
                        jsonElement.GetProperty("Username").GetString() ?? ""
                    );
                }
            }
            catch { }

            return null;
        }

        // إعداد Cookie لسلة التسوق
        public void SetCartCookie(List<int> productIds)
        {
            SetJsonCookie("ShoppingCart", productIds, 7 * 24 * 60); 
        }

        // استرجاع بيانات سلة التسوق
        public List<int> GetCartItems()
        {
            return GetCookie<List<int>>("ShoppingCart") ?? new List<int>();
        }

        // إعداد Cookie للتفضيلات
        public void SetUserPreferences(object preferences)
        {
            SetJsonCookie("UserPreferences", preferences, 365 * 24 * 60);
        }

        public void ClearAllAppCookies()
        {
            var cookiesToDelete = new[]
            {
                "RememberMe",
                "ShoppingCart",
                "UserPreferences",
                "UserSession"
            };

            foreach (var cookie in cookiesToDelete)
            {
                DeleteCookie(cookie);
            }
        }

        private string EncryptString(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        private string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey);
                aes.IV = iv;
                ICryptoTransform decryption = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryption, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

    }
    public class AuthCookieData
    {
        public string UserId { get; set; }
        public DateTime IssuedAt { get; set; }
        public string SessionId { get; set; }
    }



}

