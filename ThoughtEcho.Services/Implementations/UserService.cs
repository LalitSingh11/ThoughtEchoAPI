using ThoughtEcho.Models.RequestModels;
using ThoughtEcho.Repository.Interfaces;
using ThoughtEcho.Services.Interfaces;
using ThoughtEcho.Utilities;
using ThoughtEcho.Models;
using ThoughtEcho.Entities;

namespace ThoughtEcho.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) 
        {
            _repo = repo;
        }

        public async Task<bool> AddUser(UserRegisterModel user)
        {
            user.Password = PasswordHasher.HashPassword(user.Password);
            return await _repo.AddUser(user);
        }

        public async Task<UserCred> GetUser(UserLoginModel user)
        {
            return await _repo.GetUserByEmail(user.UserEmail);             
        }

        public async Task<bool> DoesUserExists(string Email)
        {
            return await _repo.DoesUserExists(Email);
            
        }
    }
}
