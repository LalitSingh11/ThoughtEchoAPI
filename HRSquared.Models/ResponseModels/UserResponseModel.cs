using HRSquared.Entities;
using System.ComponentModel.DataAnnotations;

namespace HRSquared.Models.ResponseModels
{
    public class UserResponseModel
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
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
