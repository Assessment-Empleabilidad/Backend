using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class User  
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
<<<<<<< HEAD
        public DateTime DateCreate { get; set; }
=======
        public DateTime DataCreate { get; set; }
        [JsonIgnore]
        public List<Loan>Loans { get; set; }
>>>>>>> cc9df8dc68e1ce04646347a2a9e16712b35ceb3c
    }
}