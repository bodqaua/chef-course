using Microsoft.EntityFrameworkCore;

namespace Chef.Models.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DatabaseContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=database;Trusted_Connection=True;Connect Timeout=30");
        }
    }
}
