using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Models
{
    public class UserLogin
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
