
using System.Security.Claims;
using Backend.Models;
using Backend.Services.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Books
{
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;  // Define el repositorio de libros

        // Constructor que inicializa el repositorio de libros
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }



        // Acción para obtener todos los libros
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

        // Acción para obtener un libro por su ID
        [HttpGet]
        [Route("api/books/{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _bookRepository.GetById(id);  // Obtiene el libro por su ID
            if  (book == null)
            {
                return NotFound("El Libro No Existe");  // Retorna un 404 si el libro no existe
            }

            return Ok(book);  // Retorna el libro encontrado
        }
    }
}