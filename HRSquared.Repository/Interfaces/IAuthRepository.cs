using HRSquared.Entities;
using HRSquared.Models.ResponseModels;

namespace HRSquared.Repository.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> AddRefreshToken(int id, RefreshTokenModel refreshToken);
        Task<RefreshToken> GetRefreshToken(int id);
    }
}
