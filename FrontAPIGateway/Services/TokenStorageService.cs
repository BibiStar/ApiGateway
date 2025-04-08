namespace FrontAPIGateway.Services
{
    // Services/TokenStorageService.cs
    public class TokenStorageService
    {
        private const string TokenKey = "JwtToken";

        public void SetToken(HttpContext httpContext, string token)
        {
            httpContext.Session.SetString(TokenKey, token);
        }

        public string? GetToken(HttpContext httpContext)
        {
            return httpContext.Session.GetString(TokenKey);
        }

        public void ClearToken(HttpContext httpContext)
        {
            httpContext.Session.Remove(TokenKey);
            httpContext.Session.Remove("UserName");
            httpContext.Session.Remove("UserRole");
        }
    }

}
