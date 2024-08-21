using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class CodeDbContext : DbContext
    {
        public CodeDbContext(DbContextOptions<CodeDbContext> options)
            : base(options)
        {
        }

        public DbSet<CodeEntity> Codes { get; set; }
    }
}
