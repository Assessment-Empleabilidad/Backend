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

namespace Backend.Services.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly BaseContext _context;
        private readonly IConfiguration _configuration;


        public UserRepository(BaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }
        public async Task<User> AuthenticateUser(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email == email);
            if (user != null)
            {
                var passCompare = new EncrypPass();
                var auth = passCompare.VerifyPassword(user.Password, password);
                if (auth)
                {
                   return user;
                }
            }
            return null;
        }

        public async Task<User> CreateUser(UserDto user)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    return null;
                }

                // Hashing de la contraseña
                var pass = new EncrypPass();
                var hashPassword = pass.HashPassword(user.Password);
                user.Password = hashPassword;

                // Creación del usuario
                var newUser = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password =  user.Password,
                    Role = "User",
                    DateCreate = DateTime.Now
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return newUser;
            }
            catch (Exception ex)
            {
                // Manejo de errores generales
                throw new Exception("An error occurred while creating the user. Please try again later.", ex);
            }
        }

        public string GenerateAuthToken(User user)
        {
             // Set up security credentials
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define token claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            
            // Configure the JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            // Return the JWT token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}