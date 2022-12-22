using Microsoft.AspNetCore.Mvc;
using HRSquared.Services.Interfaces;
using HRSquared.Models.UserModels;
using HRSquared.Entities;

namespace HRSquared.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseModel>> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return await _authService.RegisterUser(user);           
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponseModel>> Login(UserLoginModel user)
        {
            if(!await _userService.DoesUserExists(user.Username))
            {
                return BadRequest();
            }
            return await _authService.LoginUser(user);
        }
    }
}
