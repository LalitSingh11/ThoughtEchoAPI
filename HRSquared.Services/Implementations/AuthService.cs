using HRSquared.Entities;
using HRSquared.Models.UserModels;
using HRSquared.Services.Interfaces;
using HRSquared.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HRSquared.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthService(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        public async Task<UserResponseModel> RegisterUser(User user)
        {
            var token = CreateJWT(user);
            await _userService.AddUser(user);
            return new UserResponseModel(user, token);
        }

        public async Task<UserResponseModel> LoginUser(UserLoginModel user)
        {
            var user1 = await _userService.GetUser(user);
            if (PasswordHasher.VerifyHashedPassword(user.Password, user1.Password))
            {
                var token = CreateJWT(user1);
                return new UserResponseModel(user1, token);
            }
            return null;
        }

        public string CreateJWT(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Email)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("SecretKey").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims:claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
