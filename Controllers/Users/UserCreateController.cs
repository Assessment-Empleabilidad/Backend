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
    // Controlador para la creación de usuarios
    public class UserCreateController : ControllerBase
    {
        private readonly IUserRepository _userService; // Define el repositorio de usuarios

        // Constructor que inicializa el repositorio de usuarios
        public UserCreateController(IUserRepository userService)
        {
            _userService = userService;
        }

        // Acción para crear un nuevo usuario
        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto user)
        {
            // Verifica si el modelo es válido
            if (!ModelState.IsValid)
            {
                return StatusCode(400, new { Message = "All inputs are necessary", StatusCode = 400, ModelState });
            }

            try
            {
                // Intenta crear el nuevo usuario
                var newUser = await _userService.CreateUser(user);

                // Verifica si el usuario ya existe (conflicto de correo electrónico)
                if (newUser == null)
                {
                    // Retorna un 409 Conflict si el correo ya está en uso
                    return Conflict(new { Message = "Email is already in use", StatusCode = 409, DateTime.UtcNow });
                }

                // Retorna un estado de éxito 201 si el usuario se creó exitosamente
                return StatusCode(201, new { Message = "User created successfully", DateTime.UtcNow, StatusCode = 201 });
            }
            catch (ValidationException ve)
            {
                // En caso de una excepción de validación, retorna un estado de error 400 con el mensaje de la excepción
                return BadRequest(ve.Message);
            }
            catch (Exception e)
            {
                // En caso de una excepción general, retorna un estado de error 500 con el mensaje de la excepción
                return StatusCode(500, e.Message);
            }
        }
    }
}