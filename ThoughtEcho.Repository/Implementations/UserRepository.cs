using AutoMapper;
using ThoughtEcho.Entities;
using ThoughtEcho.Models.RequestModels;
using ThoughtEcho.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ThoughtEcho.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ThoughtEchoDbContext _db;
        private readonly IMapper _mapper;
        public UserRepository(ThoughtEchoDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<UserCred> GetUserByEmail(string Email)
        {
            return await _db.UserCreds.FirstOrDefaultAsync(x => x.Email == Email);
        }

        public async Task<UserCred> GetUserById(int id)
        {
            return await _db.UserCreds.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> AddUser(UserRegisterModel user)
        {
             UserCred user1 = _mapper.Map<UserCred>(user);
             await _db.UserCreds.AddAsync(user1);
             return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DoesUserExists(string email)
        {
            return await _db.UserCreds.AnyAsync(x => x.Email == email);
        }
    }
}
