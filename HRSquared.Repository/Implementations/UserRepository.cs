using HRSquared.Entities;
using HRSquared.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRSquared.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly HrsquaredDbContext _db;
        public UserRepository(HrsquaredDbContext db)
        {
            _db = db;
        }
        public async Task<UserCred> GetUserByEmail(string Email)
        {
            return await _db.UserCreds.FirstOrDefaultAsync(x => x.Email == Email);
        }

        public async Task<UserCred> GetUserById(int id)
        {
            return await _db.UserCreds.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> AddUser(UserCred user)
        {
             await _db.UserCreds.AddAsync(user);
             return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DoesUserExists(string email)
        {
            return await _db.UserCreds.AnyAsync(x => x.Email == email);
        }
    }
}
