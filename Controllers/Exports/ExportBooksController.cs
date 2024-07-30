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
        private readonly BaseContext _context;  // Define el contexto de base de datos

        // Constructor que inicializa el contexto de la base de datos
        public ExportBooksController(BaseContext context)
        {
            _context = context;
        }

        // Acción para exportar libros a un archivo PDF
        [HttpGet]
        [Route("api/export/books/pdf")]
        public IActionResult ExportPDF()
        {
            MemoryStream workStream = new MemoryStream();  // Crea un flujo de memoria
            Document document = new Document();  // Crea un nuevo documento PDF

            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;  // No cerrar el flujo al cerrar el documento

            document.Open();  // Abre el documento para escritura

            // Definición de fuentes
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, Font.NORMAL, BaseColor.BLACK);
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.NORMAL, BaseColor.WHITE);

            var books = _context.Books.ToList();

            if (books == null || books.Count == 0)
            {
                return NotFound("No se encontraron libros");
            }

            // Agrega el título del documento
            Paragraph title = new Paragraph("Libros", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            // Creación de la tabla
            PdfPTable table = new PdfPTable(6);  // Tabla con 6 columnas
            table.WidthPercentage = 100;  // Ancho de la tabla al 100%
            table.SpacingAfter = 50;  // Espacio después de la tabla

            // Agregar encabezados de la tabla
            table.AddCell("ID");
            table.AddCell("Title");
            table.AddCell("Author");
            table.AddCell("Genre");
            table.AddCell("Publication Date");
            table.AddCell("Status");

            // Agregar datos a la tabla
            foreach (var book in books)
            {
                table.AddCell(book.Id.ToString());
                table.AddCell(book.Title);
                table.AddCell(book.Author);
                table.AddCell(book.Genre);
                table.AddCell(book.PublicationDate.ToString("yyyy/MM/dd"));
                table.AddCell(book.Status);
            }

            document.Add(table);  // Agrega la tabla al documento

            document.Close();  // Cierra el documento

            // Configuración del resultado PDF
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            var fileName = $"Historial de libros";
            return File(workStream, "application/pdf", fileName);  // Retorna el archivo PDF
        }

        // Acción para exportar libros a un archivo Excel
        [HttpGet]
        [Route("book/export")]
        public async Task<IActionResult> ExportToExcel()
        {
            var libros = await _context.Books.ToListAsync();

            if (libros == null || libros.Count == 0)
            {
                return NotFound("No se encontraron libros");
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Libros");  // Crea una nueva hoja de cálculo

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
                    worksheet.Cells[i + 2, 5].Value = libro.PublicationDate.ToString("yyyy/MM/dd");
                    worksheet.Cells[i + 2, 6].Value = libro.CopiesAvailable;
                    worksheet.Cells[i + 2, 7].Value = libro.Status;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);  // Guarda el paquete de Excel en el flujo de memoria
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Libro.xlsx");  // Retorna el archivo Excel
            }
        }
    }
}