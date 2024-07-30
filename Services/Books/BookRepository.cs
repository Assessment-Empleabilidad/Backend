
using System.Security.Claims;
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

        public void Add(Book book, string userRole)
        {
            if (userRole != "Admin")
            {
                throw new UnauthorizedAccessException("Only administrators can add books.");
            }

            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Delete(int id, string UserRole)
        {
            if (UserRole != "Admin")
            {
                throw new UnauthorizedAccessException("Only administrators can delete books.");
            }
            var book = _context.Books.Find(id);
            if (book != null)
            {
                book.Status = "Inactive";
                _context.Books.Update(book);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Book> GetAll(string UserRole)
        {
            if (UserRole != "Admin")
            {
                throw new UnauthorizedAccessException("Only administrators obtain books.");
            }
            var books = _context.Books.ToList();
            return books;
        }

        // Obtiene un libro específico por su ID
        public Book GetById(int id)
        {
            var book = _context.Books.FirstOrDefault(book => book.Id == id); // Busca el libro por su ID
            return book; // Retorna el libro encontrado o null si no existe
        }

        public void Update(Book book, string UserRole)
        {
            if (UserRole != "Admin")
            {
                throw new UnauthorizedAccessException("Only administrators can update books.");
            }
            _context.Books.Update(book);
            _context.SaveChanges();
        }
    }
}