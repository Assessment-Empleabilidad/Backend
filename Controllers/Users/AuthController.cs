using Backend.Dto.User;
using Backend.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Users
{
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userService;
        public AuthController(IUserRepository userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLogingDto userLoginDto)
        {
            if (string.IsNullOrWhiteSpace(userLoginDto.Email) || string.IsNullOrWhiteSpace(userLoginDto.Password))
                return StatusCode(400, new{Message = "Email or password must not be empty", StatusCode = 400});
            try
            {
                var user = await _userService.AuthenticateUser(userLoginDto.Email, userLoginDto.Password);
                if (user == null)
                {
                    return StatusCode(400, new { Message = "User or password incorrect", StatusCode = 400});
                }
                var token = _userService.GenerateAuthToken(user);
                return StatusCode(200, new { Message = "Access valid", Token = token, DateTime.UtcNow, Rol = user.Role, StatusCode = 200 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500, DateTime.UtcNow });
            }
        }
    }
}