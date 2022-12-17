using Manago.Services.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Manago.Services.Identity.DbContexts
{
    public class ApplicationDbContexts:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContexts(DbContextOptions<ApplicationDbContexts> options) : base(options)
        {

        }
    
    }
}
