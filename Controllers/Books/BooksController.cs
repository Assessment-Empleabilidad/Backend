
using Backend.Models;
using Backend.Services.Books;
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
        public IEnumerable<Book> GetBooks()
        {
            return _bookRepository.GetAll();  // Retorna todos los libros del repositorio
        }

        // Acción para obtener un libro por su ID
        [HttpGet]
        [Route("api/books/{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _bookRepository.GetById(id);  // Obtiene el libro por su ID
            if (book == null)
            {
                return NotFound("El Libro No Existe");  // Retorna un 404 si el libro no existe
            }

            return Ok(book);  // Retorna el libro encontrado
        }
    }
}