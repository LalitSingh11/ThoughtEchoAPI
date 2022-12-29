using System.ComponentModel.DataAnnotations;

namespace HRSquared.Models.RequestModels
{
    public class UserLoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
