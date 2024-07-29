using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Backend.Models
{
    public class Book  
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public DateTime PublicationDate { get; set; }
        public int CopiesAvailable { get; set; }
        public string Status { get; set; }
        [JsonIgnore]
        public List<Loan> Loans { get; set; }
    }
}