using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Backend.Controllers.Exports
{
    public class ExportClientsController : ControllerBase
    {
        private readonly BaseContext _context;  // Define el contexto de base de datos

        // Constructor que inicializa el contexto de la base de datos
        public ExportClientsController(BaseContext context)
        {
            _context = context;
        }

        // Acción para exportar clientes a un archivo Excel
        [HttpGet]
        [Route("clients/export")]
        public async Task<IActionResult> ExportToExcel()
        {
            var users = await _context.Users.Where(s => s.Role == "User").ToListAsync();  // Obtiene la lista de usuarios con rol "User"

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Clientes");  // Crea una nueva hoja de cálculo

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
                    worksheet.Cells[i + 2, 1].Value = user.Id;  // Agrega el Id del usuario
                    worksheet.Cells[i + 2, 2].Value = user.Name;  // Agrega el Nombre del usuario
                    worksheet.Cells[i + 2, 3].Value = user.Email;  // Agrega el Correo del usuario
                    worksheet.Cells[i + 2, 4].Value = user.Role;  // Agrega el Rol del usuario
                    worksheet.Cells[i + 2, 5].Value = user.DateCreate.ToString("yyyy-MM-dd");  // Agrega la Fecha de registro del usuario
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);  // Guarda el paquete de Excel en el flujo de memoria
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Clientes.xlsx");  // Retorna el archivo Excel
            }
        }
    }
}