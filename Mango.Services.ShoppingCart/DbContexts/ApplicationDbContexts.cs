using Mango.Services.ShoppingCartAPI.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCart.DbContexts
{
    public class ApplicationDbContexts:DbContext
    {
        public ApplicationDbContexts(DbContextOptions<ApplicationDbContexts> options) : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }

     
    }
}
