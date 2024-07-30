
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend.Models;
using Backend.Services.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Books
{
    public class BookCreateController :  ControllerBase
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
        [Authorize]
        public IActionResult Create([FromBody] Book book)
        {
            if  (book == null)
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
                _bookRepository.Add(book, userRole);
                return Ok(new { message = "The book has been created successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized("Only admin can create new books");
            }
        }
    }
}