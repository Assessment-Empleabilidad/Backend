using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    // Contexto de base de datos para Entity Framework
    public class BaseContext : DbContext
    {
        // Constructor que acepta opciones de configuración para el contexto de la base de datos
        public BaseContext(DbContextOptions<BaseContext> options) : base(options)
        {

        }

        // DbSet para la entidad User
        public DbSet<User> Users { get; set; }

        // DbSet para la entidad Book
        public DbSet<Book> Books { get; set; }

        // DbSet para la entidad Loan
        public DbSet<Loan> Loans { get; set; }

        // Configuración del modelo de la base de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la clave primaria para la entidad Book
            modelBuilder.Entity<Book>()
                .HasKey(b => b.Id);

            // Configuración de la clave primaria para la entidad Loan
            modelBuilder.Entity<Loan>()
                .HasKey(l => l.Id);

            // Configuración de la relación uno a muchos entre Book y Loan
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Loans) // Un libro puede tener muchas solicitudes de préstamo
                .WithOne(l => l.Book) // Cada solicitud de préstamo está relacionada con un libro
                .HasForeignKey(l => l.BookId); // La clave externa en Loan que referencia a Book

            base.OnModelCreating(modelBuilder); // Llama a la implementación base para aplicar cualquier otra configuración
        }
    }
}