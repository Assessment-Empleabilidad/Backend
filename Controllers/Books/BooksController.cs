
using Backend.Models;
using Backend.Services.Books;
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
        public IEnumerable<Book> GetBooks()
        {
            return _bookRepository.GetAll();
        }

        [HttpGet]
        [Route("api/books/{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _bookRepository.GetById(id);
            if(book == null)
            {
                return NotFound("El Libro No Existe");
            }

            return Ok(book);
        }
    }
}