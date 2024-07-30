using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Backend.Controllers.Exports
{
    public class ExportClientsController : ControllerBase
    {
        private readonly BaseContext _context;
        public ExportClientsController(BaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("clients/export")]
        public async Task<IActionResult> ExportToExcel()
        {
            var users = await _context.Users.Where(s=>s.Role=="User").ToListAsync();

            if (users == null || users.Count == 0)
            {
                return NotFound("No se encontraron clientes");
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Libros");

                // Agregar encabezados
                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Nombre";
                worksheet.Cells[1, 3].Value = "Correo";
                worksheet.Cells[1, 4].Value = "Role";
                worksheet.Cells[1, 5].Value = "Fecha de registro";

                // Agregar datos
                for (int i = 0; i < users.Count; i++)
                {
                    var user = users[i];
                    worksheet.Cells[i + 2, 1].Value = user.Id;
                    worksheet.Cells[i + 2, 2].Value = user.Name;
                    worksheet.Cells[i + 2, 3].Value = user.Email;
                    worksheet.Cells[i + 2, 4].Value = user.Role;
                    worksheet.Cells[i + 2, 5].Value = user.DateCreate.ToString("yyyy/MM/dd");
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Clientes.xlsx");
            }
        }
    }
}