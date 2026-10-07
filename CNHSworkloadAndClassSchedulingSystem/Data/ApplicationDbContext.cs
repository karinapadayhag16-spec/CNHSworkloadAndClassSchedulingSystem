using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CNHSworkloadAndClassSchedulingSystem.Models.Domain;

namespace CNHSworkloadAndClassSchedulingSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add DBSets for your application entities
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    }
}
