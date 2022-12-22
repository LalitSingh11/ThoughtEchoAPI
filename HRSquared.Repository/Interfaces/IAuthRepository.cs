namespace HRSquared.Repository.Interfaces
{
    public interface IAuthRepository
    {
        bool ValidateUser(string email, string password);
    }
}
