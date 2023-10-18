using AssessmentAEM.Models;
using Microsoft.EntityFrameworkCore;

namespace AssessmentAEM.Data
{
    public class PlatfromDbContext:DbContext
    {
        public PlatfromDbContext(DbContextOptions<PlatfromDbContext> options) : base(options)
        {

        }
        public DbSet<Platform> Issues { get; set; }
    }
}
