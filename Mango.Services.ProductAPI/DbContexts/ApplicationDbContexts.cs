using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
