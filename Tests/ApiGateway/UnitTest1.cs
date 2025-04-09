using ApiGateway.Controllers;
using ApiGateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }
        [Fact]
        public void Login_DeveRetornarUnauthorized_QuandoCredenciaisInvalidas()
        {
            // Arrange
            var controller = new AuthController();
            var user = new UserLogin { Username = "admin", Password = "errado" };

            // Act
            var resultado = controller.Login(user);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(resultado);
        }
    }
}