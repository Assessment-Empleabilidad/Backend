
using System.Security.Claims;
using Backend.Data;
using Backend.Models;

namespace Backend.Services.Books
{
    public class BookRepository : IBookRepository
    {
        private readonly BaseContext _context;
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

        public Book GetById(int id)
        {
            var books = _context.Books.FirstOrDefault(book => book.Id == id);
            return books;
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