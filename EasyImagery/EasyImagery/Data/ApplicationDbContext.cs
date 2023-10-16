using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EasyImagery.Models;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;

namespace EasyImagery.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EasyImagery.Models.Clinic>? Clinic { get; set; }
        public DbSet<EasyImagery.Models.Patient>? Patient { get; set; }
        public DbSet<EasyImagery.Models.Manager>? Manager { get; set; }
        public DbSet<EasyImagery.Models.Timeslot>? Timeslot { get; set; }
        public DbSet<EasyImagery.Models.Physician>? Physician { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Clinic>()
                .HasOne(c => c.Manager)
                .WithOne(m => m.Clinic)
                .HasForeignKey<Manager>(m => m.ClinicId);

            builder.Entity<Clinic>()
                .HasMany(c => c.Physicians)
                .WithOne(p => p.Clinic)
                .HasForeignKey(p => p.ClinicId);

            builder.Entity<Physician>()
                .HasMany(p => p.Timeslots)
                .WithOne(t => t.Physician)
                .HasForeignKey(t => t.PhysicianId);

            builder.Entity<Patient>()
                .HasOne(p => p.Timeslot)
                .WithOne(t => t.Patient)
                .HasForeignKey<Timeslot>(t => t.PatientId);
        }
    }
}