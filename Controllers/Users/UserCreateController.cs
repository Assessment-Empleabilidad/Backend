using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dto.User;
using Backend.Services.Users;
using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace Backend.Controllers.Users
{
    public class UserCreateController : ControllerBase
    {
         private readonly IUserRepository _userService;
        public UserCreateController(IUserRepository userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new{Message = "All inputs are neccesary", StatusCode= 400, ModelState});
            }

            try
            {
                var newUser = await _userService.CreateUser(user);

                if (newUser == null)
                {
                    // Retorna un 409 Conflict si el correo ya está en uso
                    return Conflict(new { Message = "Email is already in use", StatusCode = 409, DateTime.UtcNow });
                }

                await _userService.CreateUser(user);
                return StatusCode(201, new { Message = "User created sucesfully", DateTime.UtcNow, StatusCode = 201 });
            }
            catch (ValidationException ve)
            {
                return BadRequest(ve.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}