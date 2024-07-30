using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Backend.Data;

namespace Backend.Controllers
{
    [ApiController]  // Indica que este controlador responde a solicitudes de API
    [Route("api/[controller]")]  // Define la ruta base para las solicitudes a este controlador
    public class BookController : ControllerBase
    {
        private readonly BaseContext _context;  // Define el contexto de base de datos

        // Constructor que inicializa el contexto de la base de datos
        public BookController(BaseContext context)
        {
            _context = context;
        }

        // Acción para verificar la disponibilidad de un libro por su ID
        [HttpGet("{id}/availability")]
        public IActionResult CheckAvailability(int id)
        {
            // Busca el libro por su ID en la base de datos
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound("Book not found.");  // Retorna un 404 si no se encuentra el libro

            // Retorna el número de copias disponibles del libro
            return Ok(new { CopiesAvailable = book.CopiesAvailable });
        }

        // Acción para obtener los préstamos de un usuario por su ID
        [HttpGet("user/{userId}/loans")]
        public IActionResult GetUserLoans(int userId)
        {
            // Obtiene los préstamos del usuario, incluyendo los detalles del libro
            var loans = _context.Loans
                .Include(l => l.Book)  // Incluye la entidad relacionada Book
                .Where(l => l.UserId == userId && l.Status == "Approved")
                .Select(l => new
                {
                    BookTitle = l.Book.Title,  // Título del libro
                    DueDate = l.ReturnDate  // Fecha de devolución
                }).ToList();

            if (!loans.Any())
                return NotFound("No active loans found for the user.");  // Retorna un 404 si no se encuentran préstamos

            // Retorna la lista de préstamos del usuario
            return Ok(loans);
        }
    }
}