using System.ComponentModel.DataAnnotations;

namespace jwt_token.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
