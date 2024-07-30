using System.ComponentModel.DataAnnotations;

namespace Backend.Dto.User
{
    public class UserDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
         [Required(ErrorMessage = "Role Type is required.")]
        [RegularExpression("^(Admin|User)$", ErrorMessage = "Role must be 'Admin','User'")]
        public string Role { get; set; }

    }
}