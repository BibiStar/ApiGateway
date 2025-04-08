using ApiGateway.Models;
using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        //private readonly IConfiguration _config;

      

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLogin user)
        {
            if (user.Username == "admin" && user.Password == "123456") // Simulação de login
            {
                var token = TokenService.GenerateJwtToken(user.Username, "admin");
                return Ok(new { token });
            }
            return Unauthorized("Usuário ou senha inválidos");
        }

        
    }

}
