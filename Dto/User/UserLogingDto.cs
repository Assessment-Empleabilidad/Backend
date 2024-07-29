using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.User
{
    public class UserLogingDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}