using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace Chef.Models.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        public DatabaseContext()
        {
            try
            {
                Database.Migrate();
            }
            catch { }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=database;Trusted_Connection=True;Connect Timeout=30");
        }
    }
}
