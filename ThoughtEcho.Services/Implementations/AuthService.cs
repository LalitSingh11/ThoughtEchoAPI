using ThoughtEcho.Models.RequestModels;
using ThoughtEcho.Models.ResponseModels;
using ThoughtEcho.Services.Interfaces;
using ThoughtEcho.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using ThoughtEcho.Repository.Interfaces;
using System.Text;
using ThoughtEcho.Models;
using ThoughtEcho.Entities;
using ThoughtEcho.Utilities.EmailTemplate;

namespace ThoughtEcho.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IAuthRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IEmailService _emailService;
        public AuthService(IConfiguration configuration, IUserService userService, IAuthRepository repo, IUserRepository userRepo, IEmailService emailService)
        {
            _configuration = configuration;
            _userService = userService;
            _repo = repo;
            _userRepo = userRepo;
            _emailService = emailService;
        }
        public async Task<bool> RegisterUser(UserRegisterModel user)
        {
            int randomNumber = UtilityFunctions.GenerateRandomNumber();
            SendVerificationMail(user.Email, randomNumber);
            var response = await _userService.AddUser(user);
            await AddOtp(user.Email, randomNumber);
            return response;
        }

        public async Task<UserResponseModel> LoginUser(UserLoginModel user)
        {
            UserCred user1 = await _userService.GetUser(user);
            if (user1 == null) throw new UnauthorizedAccessException("Email is Incorrect.");
            if (!PasswordHasher.VerifyHashedPassword(user.Password, user1.Password)) throw new UnauthorizedAccessException("Password is Incorrect");
            if (!user1.IsEmailVerified) throw new UnauthorizedAccessException("Email is not verified");
            var token = CreateJWT(user1.Id, user1.Role);
            return new UserResponseModel(user1, token);            
        }
        public async Task<UserResponseModel> ConfirmEmail(OtpModel request)
        {
            var otpModelDb = await _repo.GetOtp(request.Email);  
            if (UtilityFunctions.Compare<int>(request.Otp, otpModelDb.Otp) && otpModelDb?.Expires > DateTime.Now)
            {
                UserCred user = await _userRepo.GetUserByEmail(request.Email);
                var token = CreateJWT(user.Id, user.Role);
                _repo.updateEmailVerifcation(user.Id);
                return new UserResponseModel(user, token);
            }
            throw new Exception("OTP is incorrect or expired.");
        }

        public string CreateJWT(int id, string role)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Sid, id.ToString()),
                new Claim(ClaimTypes.Role, role ?? Roles.User)
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
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).Replace(" ", "-"),
                Expires = DateTime.Now.AddDays(20)
            };
            await _repo.AddRefreshToken(id, refreshToken);
            return refreshToken;
        }

        public Tuple<int, string> GetUserDetailsFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["SecretKey"]!);
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
            string id = jwtToken.Claims.First(x => x.Type == ClaimTypes.Sid).Value;
            string role = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value;
            int userId = int.Parse(id);
            Tuple<int, string> userDetails = Tuple.Create(userId, role);
            return userDetails;
        }

        public async Task<string> GetJWTFromRefershToken(int id, string refreshToken)
        {
            UserCred user = await _userRepo.GetUserById(id);
            RefreshToken dbRefreshToken = await _repo.GetRefreshToken(id);
            if (dbRefreshToken?.Token == refreshToken && dbRefreshToken?.Expires > DateTime.Now)
            {
                return CreateJWT(user.Id, user.Role);
            }
            return null!;
        }

        public async Task<bool> SendForgotPasswordMail(string email)
        {
            var user = await _userRepo.GetUserByEmail(email);
            EmailModel request = new()
            {
                SendTo = email,
                Subject= "ThoughtEcho: Password Reset",
                Body = EmailTemplate.GetForgotPasswordTemplate(user.Id, CreateJWT(user.Id, user.Role)),
            };
            return _emailService.SendEmail(request);
        }

        public bool SendVerificationMail(string email, int randomNumber)
        {
            EmailModel request = new()
            {
                SendTo = email,
                Subject = "ThoughtEcho: Email Verification",
                Body = EmailTemplate.GetEmailVerificationTemplate(randomNumber),
            };
            return _emailService.SendEmail(request);
        }        

        public async Task<bool> AddOtp(string email, int otp)
        {
            return await _repo.AddOtp(email, otp);
        }

        public async Task<bool> ResendVerificationMail(string email)
        {
            int randomNumber = UtilityFunctions.GenerateRandomNumber();
            SendVerificationMail(email, randomNumber);
            return await _repo.UpdateOtp(email, randomNumber);
        }
    }
}
