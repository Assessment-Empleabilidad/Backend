using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    public class ExportBookController : Controller
    {
        private readonly BaseContext _context;
        public ExportBookController(BaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/{id}/export/historybooks/pdf")]
        public IActionResult ExportPDF(int id)
        {
            MemoryStream workStream = new MemoryStream();
            Document document = new Document();

            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;

            document.Open();

            // TH
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, Font.NORMAL, BaseColor.BLACK);
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.NORMAL, BaseColor.WHITE);

            var user = _context.Users.Find(id);

            var loans = _context.Loans.Include(b => b.Book).Where(l => l.UserId == id);

            Paragraph title = new Paragraph("Historial de libros", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            // TABLE
            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;
            table.SpacingAfter = 50;

            table.AddCell("ID Loan");
            table.AddCell("Title Book");
            table.AddCell("Status");
            table.AddCell("Creation Date");
            table.AddCell("Loan Date");
            table.AddCell("Return Date");

            foreach (var loan in loans)
            {
                table.AddCell(loan.Id.ToString());
                table.AddCell(loan.Book.Title);
                table.AddCell(loan.Book.Status);
                table.AddCell(loan.CreationDate.ToString());
                table.AddCell(loan.LoanDate.ToString());
                table.AddCell(loan.ReturnDate.ToString());
            }

            document.Add(table);

            document.Close();
            
            // CONFIG PDF RESULT
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            var fileName = $"Historial de libros de {user.Name}";
            return File(workStream, "application/pdf", fileName);
        }
    }
}