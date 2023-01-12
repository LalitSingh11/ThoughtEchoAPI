using Microsoft.AspNetCore.Mvc;
using ThoughtEcho.Services.Interfaces;
using ThoughtEcho.Models.RequestModels;
using ThoughtEcho.Models.ResponseModels;
using ThoughtEcho.API.Helpers;
using System.ComponentModel.DataAnnotations;
using ThoughtEcho.API.Filters;
using ThoughtEcho.Utilities.Extensions;

namespace ThoughtEcho.API.Controllers
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
        /// <summary>
        /// Register a user
        /// </summary>
        [HttpPost("register")]
        [ValidationFilter]
        public async Task<ActionResult<bool>> Register([Required] UserRegisterModel user)
        {
            return await _authService.RegisterUser(user);
        }

        [HttpPost("login")]
        [ValidationFilter]
        public async Task<ActionResult<UserResponseModel>> Login([Required] UserLoginModel user)
        {
            UserResponseModel response = await _authService.LoginUser(user);
            SetRefreshToken(await _authService.CreateRefreshToken(response.Id));
            return Ok(response);
        }

        [HttpPost("refreshtoken/{id}")]
        public async Task<ActionResult<RefreshTokenModel>> RefreshToken(int id)
        {
            string refreshToken = HttpContext.GetCookiesInfo("refreshToken");
            string jwt = await _authService.GetJWTFromRefershToken(id, refreshToken!);
            RefreshTokenModel newRefreshToken = await _authService.CreateRefreshToken(id);
            SetRefreshToken(newRefreshToken);
            return jwt != null ? Ok(jwt) : throw new AppException("Refresh Token is incorrect or Expired.");
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> Forgotpassword([Required] [FromBody] string email)
        {
            if (!await _userService.DoesUserExists(email))
            {
                return NotFound("User does not exist.");
            }
            await _authService.SendForgotPasswordMail(email);
            return Ok("Success");
        }

        [HttpPost("confirmemail")]
        [ValidationFilter]
        public async Task<ActionResult<UserResponseModel>> ConfirmEmail([Required] [FromBody] OtpModel request)
        {
            UserResponseModel response = await _authService.ConfirmEmail(request);
            SetRefreshToken(await _authService.CreateRefreshToken(response.Id));
            return Ok(response);
        }

        [HttpPost("resendemail")]
        public async Task<ActionResult<bool>> ResendEmail(string email)
        {
            return await _authService.ResendVerificationMail(email);
        }


        #region Private methods
        private void SetRefreshToken(RefreshTokenModel refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
        #endregion
    }
}
