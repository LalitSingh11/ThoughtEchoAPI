using ThoughtEcho.Entities;
using ThoughtEcho.Models.ResponseModels;

namespace ThoughtEcho.Repository.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> AddRefreshToken(int id, RefreshTokenModel refreshToken);
        Task<RefreshToken> GetRefreshToken(int id);
        Task<bool> IsEmailVerified(string email);
        Task<bool> AddOtp(string email, int otp);
        Task<EmailVerificationOtp> GetOtp(string email);
        Task<bool> UpdateOtp(string email, int otp);
        Task<bool> updateEmailVerifcation(int id);
    }
}
