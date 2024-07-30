
using Backend.Services.Books;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers.Books
{
    public class BookDeleteController :ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BookDeleteController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpDelete]
        [Route("api/books/{id}")]
        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetById(id);
            if(book == null)
            {
                return NotFound("El Libro No Existe");
            }

            _bookRepository.Delete(id);
            return Ok(new { message = "El Libro Se Ha Cambiado de Estdo a Inactivo Correctamente" });
        }
    }
}