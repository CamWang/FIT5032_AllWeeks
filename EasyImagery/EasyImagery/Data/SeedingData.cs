using EasyImagery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EasyImagery.Data
{
    public class SeedingData
    {
        public static async Task SeedData(
            UserManager<IdentityUser> userManager,
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

            var tempu = await userManager.FindByEmailAsync("camwangs@gmail.com");
            if (tempu == null)
            {
                var tempp = new Patient
                {
                    UserName = "camwangs@gmail.com",
                    Email = "camwangs@gmail.com",
                    Name = $"Cam",
                    Address = $"Address 1",
                    City = $"City 1",
                    State = $"State 1",
                    Zip = $"3003",
                    Birthday = DateTime.Now.AddYears(-25).AddDays(5)
                };
                await userManager.CreateAsync(tempp, $"Patient0@123");
                await userManager.AddToRoleAsync(tempp, "Patient");
            }

            // Create four patients
            for (int i = 1; i <= 4; i++)
            {
                var email = $"patient{i}@easyimg.com";
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var patient = new Patient
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
                    await userManager.CreateAsync(patient, $"Patient{i}@123");
                    await userManager.AddToRoleAsync(patient, "Patient");
                }
            }

            // Create two physicians
            var physicianIds = new List<string>();
            for (int i = 1; i <= 3; i++)
            {
                var email = $"physician{i}@easyimg.com";
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var physician = new Physician
                    {
                        UserName = email,
                        Email = email,
                        Name = $"Physician {i}",
                        Description = $"Description {i}",
                        ClinicId = i % 2 + 1
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
                var admin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // Create one manager
            var managerEmail = "manager@easyimg.com";
            var managerUser = await userManager.FindByEmailAsync(managerEmail);
            if (managerUser == null)
            {
                var manager = new Manager
                {
                    UserName = managerEmail,
                    Email = managerEmail,
                    Name = "Manager 1",
                    ClinicId = 1,
                };
                await userManager.CreateAsync(manager, "Manager@123");
                await userManager.AddToRoleAsync(manager, "Manager");
            }

            // Create ten timeslots for each clinic
            for (int j = 1; j <= 20; j++)
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
                        PhysicianId = physicianIds[j % 3],
                        PatientId = null
                    };
                    context.Timeslot.Add(timeslot);
                }
            }
            await context.SaveChangesAsync();
        }

    }
}
