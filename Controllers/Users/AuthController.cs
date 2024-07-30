using Backend.Dto.User;
using Backend.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Users
{
    // Controlador para la autenticación de usuarios
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userService; // Define el repositorio de usuarios

        // Constructor que inicializa el repositorio de usuarios
        public AuthController(IUserRepository userService)
        {
            _userService = userService;
        }

        // Acción para el inicio de sesión de usuarios
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLogingDto userLoginDto)
        {
            // Verifica si el email o la contraseña están vacíos
            if (string.IsNullOrWhiteSpace(userLoginDto.Email) || string.IsNullOrWhiteSpace(userLoginDto.Password))
                return StatusCode(400, new { Message = "Email or password must not be empty", StatusCode = 400 });

            try
            {
                // Autentica al usuario con el email y la contraseña proporcionados
                var user = await _userService.AuthenticateUser(userLoginDto.Email, userLoginDto.Password);

                // Si el usuario no existe, retorna un estado de error 400
                if (user == null)
                {
                    return StatusCode(400, new { Message = "User or password incorrect", StatusCode = 400 });
                }

                // Genera un token de autenticación para el usuario
                var token = _userService.GenerateAuthToken(user);

                // Retorna un estado de éxito 200 con el token de autenticación
                return StatusCode(200, new { Message = "Access valid", Token = token, DateTime.UtcNow, Rol = user.Role, StatusCode = 200 });
            }
            catch (Exception ex)
            {
                // En caso de una excepción, retorna un estado de error 500 con el mensaje de la excepción
                return StatusCode(500, new { Message = ex.Message, StatusCode = 500, DateTime.UtcNow });
            }
        }
    }
}