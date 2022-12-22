using HRSquared.Repository.Interfaces;

namespace HRSquared.Repository.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IUserRepository _repo;
        public AuthRepository(IUserRepository repo) 
        {
            _repo = repo;
        }

        public bool ValidateUser(string email, string password)
        {
            return true;
        }
    }
}
