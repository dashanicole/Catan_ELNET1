using Microsoft.EntityFrameworkCore;
using CATAN_MachineProblem2.Models;

namespace CATAN_MachineProblem2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<StudentProfile> StudentProfiles { get; set; }
    }
}