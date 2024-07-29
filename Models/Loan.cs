namespace Backend.Models
{
    public class Loan  
    {
        public int Id { get; set; }
        public datetime CreationDate { get; set; }
        public datetime LoanDate { get; set; }
        public datetime ReturnDate { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}