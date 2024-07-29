namespace Backend.Services.Mailersend
{
    public interface IMailersendServices
    {
        public void SendMail(string para, string usuario);
        public void SendLoanNotification(string para, string user, string admin, string genre, string title, string autor, string loanDate, string returnDate);
    }
}