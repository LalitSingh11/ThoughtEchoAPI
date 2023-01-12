using BCryptNet = BCrypt.Net.BCrypt;
namespace ThoughtEcho.Utilities
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCryptNet.HashPassword(password);
        }

        public static bool VerifyHashedPassword(string providedPassword, string hashedPassword)
        {
            return BCryptNet.Verify(providedPassword, hashedPassword);             
        }
    }
}
