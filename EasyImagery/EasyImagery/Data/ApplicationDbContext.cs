using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EasyImagery.Models;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;

namespace EasyImagery.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Clinic>? Clinic { get; set; }
        public DbSet<Timeslot>? Timeslot { get; set; }
        public DbSet<ApplicationUser>? Physician { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Clinic>()
                .HasOne(c => c.Manager)
                .WithOne(u => u.ManagerClinic)
                .HasForeignKey<ApplicationUser>(u => u.ManagerClinicId)
                .HasPrincipalKey<Clinic>(c => c.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Clinic>()
                .HasMany(c => c.Physicians)
                .WithOne(u => u.PhysicianClinics)
                .HasForeignKey(u => u.PhysicianClinicId);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.PhysicianTimeslots)
                .WithOne(t => t.Physician)
                .HasForeignKey(t => t.PhysicianId);

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.PatientTimeslot)
                .WithOne(t => t.Patient)
                .HasForeignKey<Timeslot>(t => t.PatientId);
        }
    }
}