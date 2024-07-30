
using Backend.Data;
using Backend.Models;

namespace Backend.Services.Books
{
    // Implementación del repositorio de libros que maneja las operaciones de datos
    public class BookRepository : IBookRepository
    {
        private readonly BaseContext _context; // Contexto de base de datos para las operaciones

        // Constructor que inicializa el contexto de base de datos
        public BookRepository(BaseContext context)
        {
            _context = context;
        }

        // Agrega un nuevo libro a la base de datos
        public void Add(Book book)
        {
            _context.Books.Add(book); // Añade el libro a la colección de Books
            _context.SaveChanges(); // Guarda los cambios en la base de datos
        }

        // Elimina un libro de la base de datos por su ID (cambia el estado a "Active" en lugar de eliminar)
        public void Delete(int id)
        {
            var book = _context.Books.Find(id); // Busca el libro por su ID
            if (book != null)
            {
                book.Status = "Active"; // Cambia el estado del libro a "Active" en lugar de eliminarlo
                _context.Books.Update(book); // Actualiza el libro en la base de datos
                _context.SaveChanges(); // Guarda los cambios en la base de datos
            }
        }

        // Obtiene todos los libros de la base de datos
        public IEnumerable<Book> GetAll()
        {
            var books = _context.Books.ToList(); // Obtiene la lista de todos los libros
            return books; // Retorna la lista de libros
        }

        // Obtiene un libro específico por su ID
        public Book GetById(int id)
        {
            var book = _context.Books.FirstOrDefault(book => book.Id == id); // Busca el libro por su ID
            return book; // Retorna el libro encontrado o null si no existe
        }

        // Actualiza un libro existente en la base de datos
        public void Update(Book book)
        {
            _context.Books.Update(book); // Actualiza el libro en la base de datos
            _context.SaveChanges(); // Guarda los cambios en la base de datos
        }
    }
}