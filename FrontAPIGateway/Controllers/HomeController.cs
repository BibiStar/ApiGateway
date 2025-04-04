using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FrontAPIGateway.Models;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using FrontAPIGateway.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FrontAPIGateway.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TokenStorageService _tokenStorage;


    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, TokenStorageService tokenStorage)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _tokenStorage = tokenStorage;
    }

    public async Task<IActionResult> Index()
    {
        var token = HttpContext.Session.GetString("JWT");

        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Account");

        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("https://localhost:7080/api/protegido");
        var data = await response.Content.ReadAsStringAsync();

        ViewBag.ApiResponse = data;
        return View();
    }



    /*[Authorize] */// exige token
    public async Task<IActionResult> Protegido()
    {
        var tokenString = _tokenStorage.GetToken(HttpContext);

        if (string.IsNullOrEmpty(tokenString))
            return RedirectToAction("Login", "Account");

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(tokenString);

        var username = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "unique_name")?.Value;
        var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role")?.Value;

        ViewBag.Username = username;
        ViewBag.Role = role;

        return View();
    }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
