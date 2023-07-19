using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions options) : base(options)
        {
            
        } 

       public DbSet<Patron> Patrons { get; set; } 
    }
}