using HRSquared.Entities;
using HRSquared.Models.RequestModels;

namespace HRSquared.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserCred> GetUser(UserLoginModel user);
        Task<UserCred> AddUser(UserCred user);
        Task<bool> DoesUserExists(string Email);
    }
}
