using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Dto.User;
using Backend.Models;

namespace Backend.Services.Users
{
    public interface IUserRepository
    {
        Task<User> CreateUser(UserDto user);
        Task<User> AuthenticateUser(string email, string password);
        string GenerateAuthToken(User user);
    }
}