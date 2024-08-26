using ThoughtEcho.Entities;
using ThoughtEcho.Models.RequestModels;
using ThoughtEcho.Models.ResponseModels;

namespace ThoughtEcho.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(UserRegisterModel user);
        Task<UserResponseModel> LoginUser(UserLoginModel user);
        string CreateJWT(int id, string role);
        Task<RefreshTokenModel> CreateRefreshToken(int id);
        Task<string> GetJWTFromRefershToken(int id, string refershToken);
        Tuple<int, string> GetUserDetailsFromToken(string token);
        Task<bool> SendForgotPasswordMail(string email);
        Task<UserResponseModel> ConfirmEmail(OtpModel request);
        bool SendVerificationMail(string email, int randomNumber);
        Task<bool> AddOtp(string email, int randomNumber);
        Task<bool> ResendVerificationMail(string email);
    }
}
