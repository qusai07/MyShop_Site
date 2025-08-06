
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MyShop_Site.Models;
using System.Security.Claims;

namespace MyShop_Site.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedSessionStorage _sessionStorage;
        private readonly UserService _userService;

        public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage, UserService userService)
        {
            _sessionStorage = sessionStorage;
            _userService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var userIdResult = await _sessionStorage.GetAsync<int>("userId");
                
                if (userIdResult.Success)
                {
                    var user = await _userService.GetUserByIdAsync(userIdResult.Value);
                    if (user != null)
                    {
                        var claims = new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim("CompanyName", user.CompanyName)
                        };

                        var identity = new ClaimsIdentity(claims, "custom");
                        var principal = new ClaimsPrincipal(identity);
                        
                        return new AuthenticationState(principal);
                    }
                }
            }
            catch
            {
                // If there's an error reading from session storage, treat as anonymous
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public async Task MarkUserAsAuthenticatedAsync(User user)
        {
            await _sessionStorage.SetAsync("userId", user.Id);
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("CompanyName", user.CompanyName)
            };

            var identity = new ClaimsIdentity(claims, "custom");
            var principal = new ClaimsPrincipal(identity);
            
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task MarkUserAsLoggedOutAsync()
        {
            await _sessionStorage.DeleteAsync("userId");
            
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }
    }
}
