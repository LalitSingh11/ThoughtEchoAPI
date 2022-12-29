using HRSquared.Entities;

namespace HRSquared.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserCred> GetUserByEmail(string Email);
        Task<UserCred> GetUserById(int id);
        Task<bool> AddUser(UserCred user);
        Task<bool> DoesUserExists(string email);
    }
}
