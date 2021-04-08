using Microsoft.EntityFrameworkCore;

namespace Chef.Models.Database
{
    public interface IDatabaseContext
    {
        DbSet<Product> Products { get; set; }
    }
}