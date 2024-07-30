using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    public class ExportHistoryBookController : Controller
    {
        private readonly BaseContext _context;
        public ExportHistoryBookController(BaseContext context)
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
        [HttpGet]
        [Route("{Id}/export/historybooks/excel")]
        public async Task<IActionResult> ExportToExcel(int Id)
        {
            var libros = await _context.Loans.Include(b => b.Book).ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Libros");

                // Agregar encabezados
                worksheet.Cells[1, 1].Value = "Id Prestamo";
                worksheet.Cells[1, 2].Value = "Titulo Libro";
                worksheet.Cells[1, 3].Value = "Estado prestamo";
                worksheet.Cells[1, 4].Value = "Fecha creacion prestamo";
                worksheet.Cells[1, 5].Value = "Fecha prestamo";
                worksheet.Cells[1, 6].Value = "Fecha de retorno prestamo";  // Cambiado el índice de columna

                // Agregar datos
                for (int i = 0; i < libros.Count; i++)
                {
                    var libro = libros[i];
                    worksheet.Cells[i + 2, 1].Value = libro.Id;                  // Id del préstamo
                    worksheet.Cells[i + 2, 2].Value = libro.Book.Title;          // Título del libro (asumiendo que `Book` tiene una propiedad `Title`)
                    worksheet.Cells[i + 2, 3].Value = libro.Status;              // Estado del préstamo
                    worksheet.Cells[i + 2, 4].Value = libro.CreationDate.ToString("yyyy-MM-dd"); // Fecha de creación del préstamo
                    worksheet.Cells[i + 2, 5].Value = libro.LoanDate.ToString("yyyy-MM-dd");     // Fecha de préstamo
                    worksheet.Cells[i + 2, 6].Value = libro.ReturnDate.ToString("yyyy-MM-dd"); // Fecha de retorno del préstamo (usando el operador de null condicional para evitar errores si es null)
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Estudiantes.xlsx");
            }
        }
    }
}
