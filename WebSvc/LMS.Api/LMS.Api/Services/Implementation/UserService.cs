using LMS.Api.Models;
using LMS.Api.Repository.Implementation;
using LMS.Api.Repository.Interface;
using LMS.Api.Services.Contract;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMS.Api.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration config;

        public UserService(IUserRepository _userRepository, IConfiguration config)
        {
            this.userRepository = _userRepository;
            this.config = config;
        }

        public async Task<string> Register(Users users)
        {
           await userRepository.Register(users);
           return users.UserID;
        }

        public async Task<string> ValidateUser(string userEmail, string userName)
        {
            return await userRepository.ValidateUser(userEmail, userName);
        }
        public string Authenticate(string username, string password)
        {
            var user = userRepository.GetUser(username, password);
            if (user == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(config["Jwt:SecretKey"]);
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Issuer = config["Jwt:Issuer"],
                Audience = config["Jwt:Issuer"],
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(config["Jwt:ExpirationInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDiscriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
