using HRSquared.Entities;

namespace HRSquared.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string Email);
        Task<bool> AddUser(User user);
        Task<bool> DoesUserExists(string email);
    }
}
