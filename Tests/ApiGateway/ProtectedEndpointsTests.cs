using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.Test
{
    public class ProtectedEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProtectedEndpointsTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ProtectedEndpoint_ReturnsSuccess_ForAuthorizedUser()
        {
            // Arrange - fazer login e pegar token
            var loginData = new
            {
                Username = "admin",
                Password = "123456"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginData);
            loginResponse.EnsureSuccessStatusCode();

            var loginResult = await loginResponse.Content.ReadFromJsonAsync<TokenResponse>();
            var token = loginResult.Token;

            // Adiciona o token no cabeçalho Authorization
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act - chama um endpoint protegido
            var response = await _client.GetAsync("/WeatherForecast"); // 

            // Assert
            response.EnsureSuccessStatusCode(); // espera 200 OK
        }

        private class TokenResponse
        {
            public string Token { get; set; }
        }
    }

}
