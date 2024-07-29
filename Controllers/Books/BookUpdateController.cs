
using Backend.Models;
using Backend.Services.Books;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Books
{
    public class BookUpdateController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BookUpdateController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpPut]
        [Route("api/books/{id}/update")]
        public IActionResult Update(int id, [FromBody] Book book)
        {
            if(book == null)
            {
                return BadRequest("El Objeto Libro es nulo");
            }

            var existingBook = _bookRepository.GetById(id);
            if(existingBook == null)
            {
                return NotFound("El Libro No Existe");
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Genre = book.Genre;
            existingBook.PublicationDate = book.PublicationDate;
            existingBook.CopiesAvailable = book.CopiesAvailable;
            existingBook.Status = book.Status;

            _bookRepository.Update(existingBook);
            return Ok(new { message = "El Libro Se Ha Actualizado Correctamente" });
        }
    }
}