using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderService.DTOs;
using Xunit;

namespace ApiGateway.Tests
{
    public class OrdersIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OrdersIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            // Simula um token JWT válido (em produção, você pode emitir dinamicamente)
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI...");
        }

        [Fact]
        public async Task GetOrders_ShouldReturnSuccess()
        {
            // Act
            var response = await _client.GetAsync("/orders");

            // Assert
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var orders = JsonSerializer.Deserialize<List<OrderDto>>(json); // ajuste para o tipo correto

            orders.Should().NotBeNull();
        }
    }
}

