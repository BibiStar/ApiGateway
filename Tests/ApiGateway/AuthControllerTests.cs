using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Test
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new System.Uri("https://localhost:7080")
            });
        }

        [Fact]
        public async Task Login_ReturnsToken_ForValidUser()
        {
            // Arrange
            var loginData = new
            {
                Username = "admin",
                Password = "123456"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginData);

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            

            Assert.NotNull(result);
            // espera que a condição passada como argumento seja falsa. Se a condição for verdadeira, o teste falhará.
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
        }

        private class TokenResponse
        {
            public string Token { get; set; }
        }
    }
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var solutionDir = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\..")); // Ajuste conforme necessário

            builder.UseContentRoot(solutionDir); // Define o ContentRoot manualmente
        }
    }
}
