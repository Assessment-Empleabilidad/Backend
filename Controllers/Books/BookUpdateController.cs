
using System.Security.Claims;
using Backend.Models;
using Backend.Services.Books;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult Update(int id, [FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest("The book object is null.");
            }

            // Verificar si el libro existe en la base de datos
            var existingBook = _bookRepository.GetById(id);
            if (existingBook == null)
            {
                return NotFound("Book not found.");
            }

            // Obtener el rol del usuario desde los claims del JWT
            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userRole == null)
            {
                return Unauthorized("User role not found.");
            }

            try
            {
                // Actualizar las propiedades del libro
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Genre = book.Genre;
                existingBook.PublicationDate = book.PublicationDate;
                existingBook.CopiesAvailable = book.CopiesAvailable;
                existingBook.Status = book.Status;

                // Llamar al servicio para actualizar el libro
                _bookRepository.Update(existingBook, userRole);
                return Ok(new { message = "The book has been updated successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}