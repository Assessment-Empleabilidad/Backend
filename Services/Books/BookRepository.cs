
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
        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                book.Status = "Active";
                _context.Books.Update(book);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Book> GetAll()
        {
            var books = _context.Books.ToList();
            return books;
        }

        public Book GetById(int id)
        {
            var books = _context.Books.FirstOrDefault(book => book.Id == id);
            return books;
        }

        public void Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }
    }
}