namespace Backend.Models
{
    public class Loan  
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}