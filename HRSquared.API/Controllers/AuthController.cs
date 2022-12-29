using Microsoft.AspNetCore.Mvc;
using HRSquared.Services.Interfaces;
using HRSquared.Entities;
using HRSquared.Models.RequestModels;
using HRSquared.Models.ResponseModels;
using HRSquared.API.Helpers;

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
        public async Task<ActionResult<UserResponseModel>> Register(UserCred user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserResponseModel response = await _authService.RegisterUser(user);
            SetRefreshToken(await _authService.CreateRefreshToken(response.Id));
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponseModel>> Login(UserLoginModel user)
        {
            if(!await _userService.DoesUserExists(user.Username))
            {
                return BadRequest("User Does Not Exist");
            }
            UserResponseModel response = await _authService.LoginUser(user); 
            SetRefreshToken(await _authService.CreateRefreshToken(response.Id));
            return Ok(response);
            
        }

        [HttpPost("refreshtoken/{id}")]
        public async Task<ActionResult<RefreshTokenModel>> RefreshToken(int id)
        {
            string refreshToken = Request.Cookies["refreshToken"];
            var jwt = await _authService.GetJWTFromRefershToken(id, refreshToken);
            var newRefreshToken = await _authService.CreateRefreshToken(id);
            SetRefreshToken(newRefreshToken);            
            return jwt != null ? Ok(jwt) : throw new AppException("Refresh Token is incorrect or Expired.");
        }

        private void SetRefreshToken(RefreshTokenModel refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
