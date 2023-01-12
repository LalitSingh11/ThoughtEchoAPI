using System.Security.Cryptography;

namespace ThoughtEcho.Utilities.EmailTemplate
{
    public class EmailTemplate
    {
        public static string GetForgotPasswordTemplate(int id, string jwt)
        {
            return $"<h1><a href=\"id={id}/token={jwt}\">Click to Login without Password</a></h1><br><h1><a href=\"id={id}/token={jwt}\">Click to Reset Password</a></h1>";
        }

        public static string GetEmailVerificationTemplate(int randomNumber)
        {
            return $"<h2>Your OTP is :{randomNumber}.</h2><br><h3>This will Expire in 10 minutes.</h3>";
        }
    }
}
