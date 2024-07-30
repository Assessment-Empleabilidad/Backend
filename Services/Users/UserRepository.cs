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
        private readonly BaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMailersendServices _mailersendServices;

        public UserRepository(BaseContext context, IConfiguration configuration, IMailersendServices mailersendServices)
        {
            _context = context;
            _configuration = configuration;
            _mailersendServices = mailersendServices;

        }
        public async Task<User> AuthenticateUser(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
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
                    Password = user.Password,
                    Role = user.Role,
                    DateCreate = DateTime.Now
                };
                _context.Users.Add(newUser);
                _mailersendServices.SendMail(user.Email, user.Name);
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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}