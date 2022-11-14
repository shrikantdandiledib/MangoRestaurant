using Mango.Services.ProductAPI.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.DbContexts
{
    public class ApplicationDbContexts:DbContext
    {
        public ApplicationDbContexts(DbContextOptions<ApplicationDbContexts> options) : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
    }
}
