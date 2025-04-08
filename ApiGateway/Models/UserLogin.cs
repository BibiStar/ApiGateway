using System.ComponentModel.DataAnnotations;

namespace ApiGateway.Models
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
