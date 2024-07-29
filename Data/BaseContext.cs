using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class BaseContext : DbContext
    {
        public BaseContext (DbContextOptions<BaseContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
    }
}