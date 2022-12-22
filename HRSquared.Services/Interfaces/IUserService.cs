using HRSquared.Entities;
using HRSquared.Models.UserModels;

namespace HRSquared.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser(UserLoginModel user);
        Task<User> AddUser(User user);
        Task<bool> DoesUserExists(string Email);
    }
}
