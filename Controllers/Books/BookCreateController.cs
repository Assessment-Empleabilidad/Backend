
using Backend.Models;
using Backend.Services.Books;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Books
{
    public class BookCreateController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;  // Define el repositorio de libros

        // Constructor que inicializa el repositorio de libros
        public BookCreateController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // Acción para crear un nuevo libro
        [HttpPost]
        [Route("api/books/create")]
        public IActionResult Create([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("El Objeto libro es nulo");  // Retorna un 400 si el objeto libro es nulo
            }

            _bookRepository.Add(book);  // Agrega el libro al repositorio
            return Ok(new { message = "El Libro Se Ha Creado Correctamente" });  // Retorna un mensaje de éxito
        }
    }
}