using FrontAPIGateway.Models;
using Microsoft.AspNetCore.Mvc;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FrontAPIGateway.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();
            var loginData = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7080/api/auth/login", loginData); // URL do seu gateway

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(content);
                var token = json.RootElement.GetProperty("token").GetString();

                // Armazenar o token no cookie
                Response.Cookies.Append("jwt_token", token!, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(2)
                });

                return RedirectToAction("Index", "Home"); // ou para outra área protegida
            }

            ModelState.AddModelError(string.Empty, "Login inválido");
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token");
            return RedirectToAction("Login");
        }
    }
}
