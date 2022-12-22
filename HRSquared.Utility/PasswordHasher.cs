using BCryptNet = BCrypt.Net.BCrypt;
namespace HRSquared.Utility
{
    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
           var a =  BCryptNet.HashPassword(password);
            return a;
        }

        public static bool VerifyHashedPassword(string providedPassword, string hashedPassword)
        {
            var a = BCryptNet.Verify(providedPassword, hashedPassword);
            return a;
        }
    }
}
