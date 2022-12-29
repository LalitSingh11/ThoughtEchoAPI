using HRSquared.Entities;
using HRSquared.Models.RequestModels;
using HRSquared.Models.ResponseModels;

namespace HRSquared.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseModel> RegisterUser(UserCred user);
        Task<UserResponseModel> LoginUser(UserLoginModel user);
        string CreateJWT(UserCred user);
        Task<RefreshTokenModel> CreateRefreshToken(int id);
        Task<string> GetJWTFromRefershToken(int id, string refershToken);
        Tuple<int, string> GetUserDetailsFromToken(string token);
    }
}
