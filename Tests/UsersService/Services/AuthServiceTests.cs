using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Services;
using Xunit;

namespace UsersService.Test.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public void Authenticate_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var service = new AuthService();
            var username = "admin";
            var password = "123456";

            // Act
            var token = service.Authenticate(username, password);

            // Assert
            Assert.NotNull(token);
            Assert.Contains(".", token); // Verifica se parece um JWT (com pontos)
        }

        [Fact]
        public void Authenticate_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var service = new AuthService();
            var username = "admin";
            var password = "wrong-password";

            // Act
            var token = service.Authenticate(username, password);

            // Assert
            Assert.Null(token);
        }
    }
}
