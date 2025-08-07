namespace MyShop_Site.Repo.Interfaces
{
    public interface ISecureCookieService
    {
        void SetSecureCookie(string key, string value, TimeSpan? expiry = null, bool essential = false);
        void SetSecureCookie<T>(string key, T value, TimeSpan? expiry = null, bool essential = false);
        string GetCookie(string key);
        T GetCookie<T>(string key) where T : class;
        void DeleteCookie(string key);
        void SetAuthenticationCookie(string userId, bool rememberMe = false);
        void ClearAuthenticationCookie();
        bool ValidateSecureCookie(string key);

    }
}
