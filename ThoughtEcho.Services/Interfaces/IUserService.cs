using ThoughtEcho.Entities;
using ThoughtEcho.Models.RequestModels;

namespace ThoughtEcho.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserCred> GetUser(UserLoginModel user);
        Task<bool> AddUser(UserRegisterModel user);
        Task<bool> DoesUserExists(string Email);
    }
}
