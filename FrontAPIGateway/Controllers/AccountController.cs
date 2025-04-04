using FrontAPIGateway.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http.Headers;
using FrontAPIGateway.Services;
using Microsoft.AspNetCore.Http;

namespace FrontAPIGateway.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenStorageService _tokenStorage;

        public AccountController(IHttpClientFactory httpClientFactory,  TokenStorageService tokenStorage)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStorage = tokenStorage;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("https://localhost:7080/api/auth/login", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
                var token = result.Token;

                // Salvar token
                HttpContext.Session.SetString("JWT", token);

                // Decodificar token para extrair informações
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                HttpContext.Session.SetString("UserName", username ?? "Desconhecido");
                HttpContext.Session.SetString("UserRole", role ?? "");
                _tokenStorage.SetToken(HttpContext, token);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Login inválido");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Limpa tudo
            
            return RedirectToAction("Login");
        }
    }
}
