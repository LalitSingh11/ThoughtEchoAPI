using System.ComponentModel.DataAnnotations;

namespace ThoughtEcho.Models.RequestModels
{
    public class UserLoginModel
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
