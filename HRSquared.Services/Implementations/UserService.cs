using HRSquared.Entities;
using HRSquared.Models.UserModels;
using HRSquared.Repository.Interfaces;
using HRSquared.Services.Interfaces;
using HRSquared.Utility;

namespace HRSquared.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) 
        {
            _repo = repo;
        }

        public async Task<User> AddUser(User user)
        {
            user.Password = PasswordHasher.HashPassword(user.Password);
            await _repo.AddUser(user);
            return await _repo.GetUserByEmail(user.Email);
        }

        public async Task<User> GetUser(UserLoginModel user)
        {
            var user1 = await _repo.GetUserByEmail(user.Username);
            return user1;
        }

        public async Task<bool> DoesUserExists(string Email)
        {
            var user = await _repo.DoesUserExists(Email);
            return user;
        }
    }
}
