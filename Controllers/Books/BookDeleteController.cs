
using Backend.Services.Books;
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
        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetById(id);  // Obtiene el libro por su ID
            if (book == null)
            {
                return NotFound("El Libro No Existe");  // Retorna un 404 si el libro no existe
            }

            _bookRepository.Delete(id);  // Elimina (cambia el estado a inactivo) el libro
            return Ok(new { message = "El Libro Se Ha Cambiado de Estado a Inactivo Correctamente" });  // Retorna un mensaje de éxito
        }
    }
}