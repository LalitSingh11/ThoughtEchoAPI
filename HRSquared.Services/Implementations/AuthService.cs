using HRSquared.Entities;
using HRSquared.Models.RequestModels;
using HRSquared.Models.ResponseModels;
using HRSquared.Services.Interfaces;
using HRSquared.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using HRSquared.Repository.Interfaces;
using System.Text;

namespace HRSquared.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IAuthRepository _repo;
        private readonly IUserRepository _userRepo;
        public AuthService(IConfiguration configuration, IUserService userService, IAuthRepository repo, IUserRepository userRepo)
        {
            _configuration = configuration;
            _userService = userService;
            _repo = repo;
            _userRepo = userRepo;
        }
        public async Task<UserResponseModel> RegisterUser(UserCred user)
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
            throw new UnauthorizedAccessException("Username or Password Incorrect");
        }

        public string CreateJWT(UserCred user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("SecretKey").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var jwtSettings = _configuration.GetSection("jwtSettings");

            var token = new JwtSecurityToken(
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RefreshTokenModel> CreateRefreshToken(int id)
        {
            RefreshTokenModel refreshToken = new()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(20)
            };
            await _repo.AddRefreshToken(id, refreshToken);
            return refreshToken;
        }

        public Tuple<int, string> GetUserDetailsFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["SecretKey"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var id = jwtToken.Claims.First(x => x.Type == ClaimTypes.Sid).Value;
            var role = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;
            int userId = int.Parse(id);
            Tuple<int, string> userDetails = Tuple.Create(userId, role);
            return userDetails;
        }

        public async Task<string> GetJWTFromRefershToken(int id, string refreshToken)
        {
            var user = await _userRepo.GetUserById(id);
            var dbRefreshToken = await _repo.GetRefreshToken(id);
            if (dbRefreshToken?.Token == refreshToken && dbRefreshToken?.Expires > DateTime.Now)
            {
                return CreateJWT(user);
            }
            return null;
        }
    }
}
