using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace BackEnd.Controllers
{
    public class ExportBooksController : ControllerBase
    {
        private readonly BaseContext _context;
        public ExportBooksController(BaseContext context)
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

        [HttpGet]
        [Route("book/export")]
        public async Task<IActionResult> ExportToExcel()
        {
            var libros = await _context.Books.ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Libros");

                // Agregar encabezados
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Titulo Libro";
                worksheet.Cells[1, 3].Value = "Autor";
                worksheet.Cells[1, 4].Value = "Genero";
                worksheet.Cells[1, 5].Value = "Fecha publicacion";
                worksheet.Cells[1, 6].Value = "Copias disponibles";
                worksheet.Cells[1, 7].Value = "Estado";

                // Agregar datos
                for (int i = 0; i < libros.Count; i++)
                {
                    var libro = libros[i];
                    worksheet.Cells[i + 2, 1].Value = libro.Id;
                    worksheet.Cells[i + 2, 2].Value = libro.Title;
                    worksheet.Cells[i + 2, 3].Value = libro.Author;
                    worksheet.Cells[i + 2, 4].Value = libro.Genre;
                    worksheet.Cells[i + 2, 5].Value = libro.PublicationDate.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 6].Value = libro.CopiesAvailable;
                    worksheet.Cells[i + 2, 7].Value = libro.Status;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Libro.xlsx");
            }
        }
    }
}