using HRSquared.Repository.Interfaces;
using HRSquared.Entities;
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
        public async Task<User> GetUserByEmail(string Email)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Email == Email);
        }

        public async Task<bool> AddUser(User user)
        {
             await _db.Users.AddAsync(user);
             return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DoesUserExists(string email)
        {
            return await _db.Users.AnyAsync(x => x.Email == email);
        }
    }
}
