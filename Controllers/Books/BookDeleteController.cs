
using System.Security.Claims;
using Backend.Services.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Books
{
    public class BookDeleteController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;  // Define el repositorio de libros

        // Constructor que inicializa el repositorio de libros
        public BookDeleteController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // Acción para eliminar (cambiar el estado a inactivo) un libro por su ID
        [HttpDelete]
        [Route("api/books/{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return BadRequest("The book object is null");
            }

            var allClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            foreach (var claim in allClaims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }

            var userRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userRole == null)
            {
                return Unauthorized("User role not found");
            }

            try
            {
                _bookRepository.Delete(id, userRole);
                return Ok(new { message = "The book has been changed of status to inactive" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized("Only admin can change the status of books");
            }
        }
    }
}