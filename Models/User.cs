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
        public DateTime DateCreate { get; set; }
        [JsonIgnore]
        public List<Loan>Loans { get; set; }

    }
}
