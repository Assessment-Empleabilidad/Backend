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
        [Route("api/export/books/pdf")]
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

            var books = _context.Books.ToList();

            Paragraph title = new Paragraph("Libros", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            // TABLE
            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;
            table.SpacingAfter = 50;

            table.AddCell("ID");
            table.AddCell("Title");
            table.AddCell("Author");
            table.AddCell("Genre");
            table.AddCell("Publication Date");
            table.AddCell("Status");

            foreach (var book in books)
            {
                table.AddCell(book.Id.ToString());
                table.AddCell(book.Title);
                table.AddCell(book.Author);
                table.AddCell(book.Genre);
                table.AddCell(book.PublicationDate.ToString());
                table.AddCell(book.Status);
            }

            document.Add(table);

            document.Close();
            
            // CONFIG PDF RESULT
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            var fileName = $"Historial de libros";
            return File(workStream, "application/pdf", fileName);
        }
    }
}