using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace BackEnd.Controllers
{
    // Controlador para exportar datos de libros
    public class ExportBookController : Controller
    {
        private readonly BaseContext _context; // Define el contexto de base de datos

        // Constructor que inicializa el contexto de la base de datos
        public ExportBookController(BaseContext context)
        {
            _context = context;
        }

        // Acción para exportar historial de libros de un usuario a un archivo PDF
        [HttpGet]
        [Route("api/{id}/export/historybooks/pdf")]
        public IActionResult ExportPDF(int id)
        {
            MemoryStream workStream = new MemoryStream(); // Define un flujo de memoria para el archivo PDF
            Document document = new Document(); // Crea un nuevo documento PDF

            PdfWriter writer = PdfWriter.GetInstance(document, workStream); // Asocia el escritor del PDF al flujo de memoria
            writer.CloseStream = false;

            document.Open(); // Abre el documento PDF

            // Define fuentes para el título y el texto en negrita
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, Font.NORMAL, BaseColor.BLACK);
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, Font.NORMAL, BaseColor.WHITE);

            var user = _context.Users.Find(id); // Busca el usuario por su ID
            var loans = _context.Loans.Include(b => b.Book).Where(l => l.UserId == id); // Obtiene los préstamos del usuario

            // Añade un título al documento
            Paragraph title = new Paragraph("Historial de libros", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20
            };
            document.Add(title);

            // Crea una tabla PDF con 6 columnas
            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;
            table.SpacingAfter = 50;

            // Añade las cabeceras de la tabla
            table.AddCell("ID Loan");
            table.AddCell("Title Book");
            table.AddCell("Status");
            table.AddCell("Creation Date");
            table.AddCell("Loan Date");
            table.AddCell("Return Date");

            // Añade los datos de los préstamos a la tabla
            foreach (var loan in loans)
            {
                table.AddCell(loan.Id.ToString());
                table.AddCell(loan.Book.Title);
                table.AddCell(loan.Book.Status);
                table.AddCell(loan.CreationDate.ToString());
                table.AddCell(loan.LoanDate.ToString());
                table.AddCell(loan.ReturnDate.ToString());
            }

            document.Add(table); // Añade la tabla al documento

            document.Close(); // Cierra el documento

            // Configura el resultado del archivo PDF
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            var fileName = $"Historial de libros de {user.Name}"; // Define el nombre del archivo
            return File(workStream, "application/pdf", fileName); // Retorna el archivo PDF
        }
    }
}