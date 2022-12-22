using HRSquared.Entities;
using HRSquared.Models.UserModels;

namespace HRSquared.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseModel> RegisterUser(User user);
        Task<UserResponseModel> LoginUser(UserLoginModel user);
        string CreateJWT(User user);
    }
}
