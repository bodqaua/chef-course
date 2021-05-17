using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Chef.Models.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<RecipeEntity> Recipes { get; set; }

        public DatabaseContext()
        {
            //try
            //{
            //    Database.Migrate();
            //} catch{
            //    Database.EnsureDeleted();
            //    Database.EnsureCreated();
            //}
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var keys = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            foreach(var foreignKey in keys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
            }
        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=database;Trusted_Connection=True;Connect Timeout=30");
        }
    }
}
