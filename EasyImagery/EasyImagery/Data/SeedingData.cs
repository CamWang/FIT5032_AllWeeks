using System;
using System.Linq;
using EasyImagery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bogus;

namespace EasyImagery.Data
{
    public class SeedingData
    {
        public static async Task SeedData(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<string> { "Admin", "Patient", "Physician", "Manager" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            if (!await context.Clinic.AnyAsync())
            {
                var clinics = new Faker<Clinic>()
                    .RuleFor(c => c.Name, f => f.Company.CompanyName())
                    .RuleFor(c => c.Description, (f, c) => $"Description for {c.Name}")
                    .RuleFor(c => c.Address, f => f.Address.FullAddress())
                    .Generate(2);

                await context.Clinic.AddRangeAsync(clinics);
            }

            if (!await context.Users.AnyAsync(u => u.UserType == "Patient"))
            {
                var patients = new Faker<ApplicationUser>()
                    .RuleFor(u => u.UserName, f => f.Internet.Email())
                    .RuleFor(u => u.Email, f => f.Internet.Email())
                    .RuleFor(u => u.Name, f => f.Name.FullName())
                    .RuleFor(u => u.Address, f => f.Address.StreetAddress())
                    .RuleFor(u => u.City, f => f.Address.City())
                    .RuleFor(u => u.State, f => f.Address.State())
                    .RuleFor(u => u.Zip, f => f.Address.ZipCode())
                    .RuleFor(u => u.EmailConfirmed, true)
                    .RuleFor(u => u.UserType, "Patient")
                    .RuleFor(u => u.Birthday, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
                    .Generate(4);

                foreach (var patient in patients)
                {
                    if (await userManager.FindByEmailAsync(patient.Email) == null)
                    {
                        await userManager.CreateAsync(patient, $"Patient@123");
                        await userManager.AddToRoleAsync(patient, "Patient");
                    }
                }
            }

            if (!await context.Users.AnyAsync(u => u.UserType == "Physician"))
            {
                var physicians = new Faker<ApplicationUser>()
                    .RuleFor(u => u.UserName, f => f.Internet.Email())
                    .RuleFor(u => u.Email, f => f.Internet.Email())
                    .RuleFor(u => u.Name, f => f.Name.FullName())
                    .RuleFor(u => u.Description, f => f.Lorem.Sentence())
                    .RuleFor(u => u.PhysicianClinicId, f => f.Random.Int(1, 2))
                    .RuleFor(u => u.UserType, "Physician")
                    .RuleFor(u => u.EmailConfirmed, true)
                    .Generate(4);

                foreach (var physician in physicians)
                {
                    if (await userManager.FindByEmailAsync(physician.Email) == null)
                    {
                        await userManager.CreateAsync(physician, $"Physician@123");
                        await userManager.AddToRoleAsync(physician, "Physician");
                    }
                }
            }

            if (!await context.Users.AnyAsync(u => u.UserType == "Admin"))
            {
                var adminEmail = "admin@easyimg.com";
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Name = "Admin",
                    UserType = "Admin"
                };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            if (!await context.Users.AnyAsync(u => u.UserType == "Manager"))
            {
                var managerEmail = "manager@easyimg.com";
                var manager = new ApplicationUser
                {
                    UserName = managerEmail,
                    Email = managerEmail,
                    EmailConfirmed = true,
                    Name = "Manager",
                    ManagerClinicId = 1,
                    UserType = "Manager"
                };
                await userManager.CreateAsync(manager, "Manager@123");
                await userManager.AddToRoleAsync(manager, "Manager");
            }

            if (!await context.Timeslot.AnyAsync())
            {
                var physicianIds = await context.Users
                    .Where(u => u.UserType == "Physician")
                    .Select(u => u.Id)
                    .ToListAsync();

                var currentHour = 8; // Start from 8am
                var currentDate = DateTime.Today; // Start from the current day

                DateTime GetNextStartDate()
                {
                    DateTime date = currentDate.AddHours(currentHour);
                    currentHour++;
                    if (currentHour > 17) // Reset to 8am for the next day if it exceeds 5pm
                    {
                        currentHour = 8;
                        currentDate = currentDate.AddDays(1);
                    }
                    return date;
                }

                var timeslotFaker = new Faker<Timeslot>()
                    .RuleFor(t => t.Description, f => f.Lorem.Sentence())
                    .RuleFor(t => t.StartDate, f => GetNextStartDate())
                    .RuleFor(t => t.EndDate, (f, t) => t.StartDate + TimeSpan.FromMinutes(45))
                    .RuleFor(t => t.PhysicianId, f => f.PickRandom(physicianIds));

                var timeslots = timeslotFaker.Generate(30);
                context.Timeslot.AddRange(timeslots);
            }


            await context.SaveChangesAsync();
        }
    }
}
