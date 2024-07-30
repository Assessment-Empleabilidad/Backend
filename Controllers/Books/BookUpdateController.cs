
using Backend.Models;
using Backend.Services.Books;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Books
{
    public class BookUpdateController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;  // Define el repositorio de libros

        // Constructor que inicializa el repositorio de libros
        public BookUpdateController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // Acción para actualizar un libro por su ID
        [HttpPut]
        [Route("api/books/{id}/update")]
        public IActionResult Update(int id, [FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("El Objeto Libro es nulo");  // Retorna un 400 si el objeto libro es nulo
            }

            var existingBook = _bookRepository.GetById(id);  // Obtiene el libro por su ID
            if (existingBook == null)
            {
                return NotFound("El Libro No Existe");  // Retorna un 404 si el libro no existe
            }

            // Actualiza los campos del libro existente con los valores del libro recibido
            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Genre = book.Genre;
            existingBook.PublicationDate = book.PublicationDate;
            existingBook.CopiesAvailable = book.CopiesAvailable;
            existingBook.Status = book.Status;

            _bookRepository.Update(existingBook);  // Actualiza el libro en el repositorio
            return Ok(new { message = "El Libro Se Ha Actualizado Correctamente" });  // Retorna un mensaje de éxito
        }
    }
}