using ThoughtEcho.Utilities;
using System.ComponentModel.DataAnnotations;

namespace ThoughtEcho.Models.RequestModels
{
    public class UserRegisterModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string Role { get; set; } = Roles.User;
    }
}
