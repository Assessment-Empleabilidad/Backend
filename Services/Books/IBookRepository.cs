
using System.Security.Claims;
using Backend.Models;

namespace Backend.Services.Books
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAll(string UserRole);
        Book GetById(int id);
        void Add(Book book, string UserRole);
        void Update(Book book, string UserRole);
        void Delete(int id, string UserRole);
        
        
    }
}