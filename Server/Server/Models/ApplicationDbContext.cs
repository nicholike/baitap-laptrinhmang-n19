using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<InfoEntity> MyEntities { get; set; }
    }
}
