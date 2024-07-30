using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Dto.User;
using Backend.Helper;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Backend.Services.Mailersend;

namespace Backend.Services.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly BaseContext _context; // Contexto de la base de datos
        private readonly IConfiguration _configuration; // Configuración para leer el archivo appsettings.json
        private readonly IMailersendServices _mailersendServices; // Servicio para enviar correos electrónicos

        // Constructor que inicializa el contexto, configuración y servicio de correo
        public UserRepository(BaseContext context, IConfiguration configuration, IMailersendServices mailersendServices)
        {
            _context = context;
            _configuration = configuration;
            _mailersendServices = mailersendServices;
        }

        // Método para autenticar al usuario
        public async Task<User> AuthenticateUser(string email, string password)
        {
            // Busca al usuario por su correo electrónico
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                // Verifica la contraseña usando la clase EncrypPass
                var passCompare = new EncrypPass();
                var auth = passCompare.VerifyPassword(user.Password, password);
                if (auth)
                {
                    return user; // Devuelve el usuario si la autenticación es exitosa
                }
            }
            return null; // Devuelve null si el usuario no se encuentra o la contraseña no es correcta
        }

        // Método para crear un nuevo usuario
        public async Task<User> CreateUser(UserDto user)
        {
            try
            {
                // Verifica si ya existe un usuario con el mismo correo electrónico
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    return null; // Devuelve null si el correo ya está en uso
                }

                // Hashea la contraseña usando la clase EncrypPass
                var pass = new EncrypPass();
                var hashPassword = pass.HashPassword(user.Password);
                user.Password = hashPassword;

                // Crea una nueva instancia del usuario
                var newUser = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Role = "User",
                    DateCreate = DateTime.Now
                };

                // Añade el usuario a la base de datos y guarda los cambios
                _context.Users.Add(newUser);
                _mailersendServices.SendMail(user.Email, user.Name); // Envía un correo de bienvenida
                await _context.SaveChangesAsync();
                return newUser; // Devuelve el nuevo usuario creado
            }
            catch (Exception ex)
            {
                // Maneja errores generales y los lanza con un mensaje personalizado
                throw new Exception("An error occurred while creating the user. Please try again later.", ex);
            }
        }

        // Método para generar un token JWT para el usuario
        public string GenerateAuthToken(User user)
        {
            // Configura las credenciales de seguridad
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define los claims (reclamaciones) del token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Configura el token JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120), // El token expira en 120 minutos
                signingCredentials: credentials
            );

            // Devuelve el token JWT como una cadena
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}