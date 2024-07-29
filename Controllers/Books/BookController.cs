using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Backend.Data;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BaseContext _context;

        public BookController(BaseContext context)
        {
            _context = context;
        }

        [HttpGet("{id}/availability")]
        public IActionResult CheckAvailability(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
                return NotFound("Book not found.");

            return Ok(new { CopiesAvailable = book.CopiesAvailable });
        }

        [HttpGet("user/{userId}/loans")]
        public IActionResult GetUserLoans(int userId)
        {
            var loans = _context.Loans
                .Include(l => l.Book)  // Include the related Book entity
                .Where(l => l.UserId == userId && l.Status == "Approved")
                .Select(l => new
                {
                    BookTitle = l.Book.Title,
                    DueDate = l.ReturnDate
                }).ToList();

            if (!loans.Any())
                return NotFound("No active loans found for the user.");

            return Ok(loans);
        }

    }
}
