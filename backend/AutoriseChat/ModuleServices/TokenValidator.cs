using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AutoriseChat.ModuleServices
{
    public class TokenValidator
    {
        private readonly IConfiguration config;

        public TokenValidator(IConfiguration config)
        {
            this.config = config;
        }

        public string GenerateToken(LoginModel login)
        {
            if (IsValidUser(login))
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, "User")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"] ?? throw new ArgumentException("Jwt key must be loaded")));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                var  tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                return tokenValue;
            }

            return "Error";
        }

        private bool IsValidUser(LoginModel login)
        {
            // Реализуйте свою логику проверки пользователя
            return login.Username == "test" && login.Password == "password";
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}