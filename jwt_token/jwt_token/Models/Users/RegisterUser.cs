

namespace jwt_token.Models
{
    public class RegisterUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string Role { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public Role rolm { get; set; }
        public string Token  { get; set; }
    }
}
