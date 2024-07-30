
using System.Security.Claims;
using Backend.Models;
using Backend.Services.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Books
{
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }


        [HttpGet]
        [Route("api/books")]
        [Authorize]
        public IActionResult GetBooks()
        {
            // Obtener el rol del usuario desde los claims del JWT
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userRole == null)
            {
                return Unauthorized("User role not found.");
            }

            try
            {
                var books = _bookRepository.GetAll(userRole);
                return Ok(books);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }


        [HttpGet]
        [Route("api/books/{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound("El Libro No Existe");
            }

            return Ok(book);
        }
    }
}