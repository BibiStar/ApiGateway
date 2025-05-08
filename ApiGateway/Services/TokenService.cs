using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGateway.Services
{
    public static class TokenService
    {
        private static readonly string jwtKey;
        private static readonly string issuer;
        private static readonly string audience;

        static TokenService()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            jwtKey = configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("Jwt:SecretKey não foi configurada.");
            issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer não foi configurada.");
            audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience não foi configurada.");
        }

        public static string GenerateJwtToken(string username, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtKey);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username), // Nome do usuário
                    new Claim(ClaimTypes.Role, role), // Role do usuário
                    new Claim(ClaimTypes.NameIdentifier, "123"), // ID do usuário (pode vir do banco)
                    new Claim(ClaimTypes.Email, $"{username}@email.com"), // E-mail do usuário (exemplo)
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Identificador único do token
                };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), // Token válido por 2 horas
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
