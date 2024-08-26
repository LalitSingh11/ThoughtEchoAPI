using ThoughtEcho.Entities;
using ThoughtEcho.Models.RequestModels;
using System.ComponentModel.DataAnnotations;

namespace ThoughtEcho.Models.ResponseModels
{
    public class UserResponseModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Token { get; set; }

        public UserResponseModel(UserCred user, string token)
        {
            Id = user.Id;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Token = token;
        }
    }
}
