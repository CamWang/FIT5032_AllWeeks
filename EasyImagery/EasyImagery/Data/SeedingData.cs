using EasyImagery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EasyImagery.Data
{
    public class SeedingData
    {
        public static async Task SeedData(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            var roles = new List<string> { "Admin", "Patient", "Physician", "Manager" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            for (int i = 1; i <= 2; i++)
            {
                var clinicName = $"Clinic {i}";
                var clinic = await context.Clinic.FirstOrDefaultAsync(c => c.Name == clinicName);
                if (clinic == null)
                {
                    clinic = new Clinic
                    {
                        Name = clinicName,
                        Description = $"Description for {clinicName}",
                        Address = $"Address for {clinicName}"
                    };
                    context.Clinic.Add(clinic);
                }
            }

            await context.SaveChangesAsync();

            for (int i = 1; i <= 4; i++)
            {
                var email = $"patient{i}@easyimg.com";
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var newUser = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        Name = $"Patient {i}",
                        Address = $"Address {i}",
                        City = $"City {i}",
                        State = $"State {i}",
                        Zip = $"Zip{i}",
                        Birthday = DateTime.Now.AddYears(-25).AddDays(i)
                    };
                    await userManager.CreateAsync(newUser, $"Patient{i}@123");
                    await userManager.AddToRoleAsync(newUser, "Patient");
                }
            }

            // Create two physicians
            var physicianIds = new List<string>();
            for (int i = 1; i <= 4; i++)
            {
                var email = $"physician{i}@easyimg.com";
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var physician = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        Name = $"Physician {i}",
                        Description = $"Description {i}",
                        PhysicianClinicId = i % 2 + 1,
                        UserType = "Physician"
                    };
                    var result = await userManager.CreateAsync(physician, $"Physician{i}@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(physician, "Physician");
                        var createdPhysician = await userManager.FindByEmailAsync(email);
                        physicianIds.Add(createdPhysician.Id);
                    }
                }
            }

            // Create one admin
            var adminEmail = "admin@easyimg.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    Name= "Admin",
                    UserName = adminEmail,
                    Email = adminEmail,
                    UserType = "Admin"
                };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Create one manager
            var managerEmail = "manager@easyimg.com";
            var managerUser = await userManager.FindByEmailAsync(managerEmail);
            if (managerUser == null)
            {
                var manager = new ApplicationUser
                {
                    UserName = managerEmail,
                    Email = managerEmail,
                    Name = "Manager 1",
                    ManagerClinicId = 1,
                    UserType = "Manager"
                };
                await userManager.CreateAsync(manager, "Manager@123");
                await userManager.AddToRoleAsync(manager, "Manager");
            }

            // Create ten timeslots for each clinic
            for (int j = 1; j <= 30; j++)
            {
                var timeslotDescription = $"Timeslot {j}";
                var timeslotExists = await context.Timeslot.AnyAsync(t => t.Description == timeslotDescription);
                if (!timeslotExists)
                {
                    var timeslot = new Timeslot
                    {
                        Description = timeslotDescription,
                        StartDate = DateTime.Now.AddHours(j),
                        EndDate = DateTime.Now.AddHours(j + 1),
                        PhysicianId = physicianIds[j % physicianIds.Count],
                    };
                    context.Timeslot.Add(timeslot);
                }
            }
            await context.SaveChangesAsync();
        }

    }
}
