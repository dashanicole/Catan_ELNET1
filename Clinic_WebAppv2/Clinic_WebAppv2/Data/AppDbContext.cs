using Clinic_WebAppv2.Models;
using Microsoft.EntityFrameworkCore;

namespace Clinic_WebAppv2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
    }
}
