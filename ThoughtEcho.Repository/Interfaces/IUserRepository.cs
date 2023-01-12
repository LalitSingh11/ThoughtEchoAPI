using ThoughtEcho.Entities;
using ThoughtEcho.Models.RequestModels;

namespace ThoughtEcho.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserCred> GetUserByEmail(string Email);
        Task<UserCred> GetUserById(int id);
        Task<bool> AddUser(UserRegisterModel user);
        Task<bool> DoesUserExists(string email);
    }
}
