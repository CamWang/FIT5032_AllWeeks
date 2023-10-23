using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyImagery.Data; // Replace with your actual namespace
using System.Linq;
using System.Threading.Tasks;
using EasyImagery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using EasyImagery.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

public class PhysicianController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender;

    public PhysicianController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, EmailSender emailSender)
    {
        _context = context;
        _userManager = userManager;
        _emailSender = emailSender;
    }

    // GET: Physicians
    public async Task<IActionResult> Index()
    {
        var physicians = await _context.Users
            .Where(u => u.UserType == "Physician")
            .Select(p => new
            {
                Physician = p,
                AverageRating = _context.Rating
                                .Where(r => r.PhysicianId == p.Id)
                                .Average(r => (double?)r.StarRating) ?? 0
            })
            .ToListAsync();

        return View(physicians);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> RatePhysician(string physicianId, int starRating)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Error", "Home", new { errorMessage = "User not authorized." });
        } else
        {
            Rating rating = new Rating
            {
                PatientId = user.Id,
                PhysicianId = physicianId,
                StarRating = starRating
            };
            await _context.Rating.AddAsync(rating);
            await _context.SaveChangesAsync();
        }

        // Add the rating to your database (this step might vary depending on your ORM or database layer)

        return RedirectToAction("Index");
    }

    [Authorize(Roles ="Physician")]
    public async Task<IActionResult> ListPatients()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null || currentUser.UserType != "Physician")
        {
            return RedirectToAction("Error", "Home", new { errorMessage = "User not authorized." });
        }

        var patients = _context.Timeslot
            .Where(t => t.PhysicianId == currentUser.Id && t.Patient != null)
            .Select(t => t.Patient)
            .Distinct()
            .ToList();

        return View(patients);
    }

    [HttpPost]
    public async Task<IActionResult> SendBulkEmails(string subject, string message, List<string> selectedPatients)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null || currentUser.UserType != "Physician")
        {
            return RedirectToAction("Error", "Home", new { errorMessage = "User not authorized." });
        }

        if (selectedPatients == null || !selectedPatients.Any())
        {
            // Handle the case where no patients were selected, maybe show an error message.
            return RedirectToAction("ListPatients", new { error = "No patients were selected." });
        }

        var patientsToEmail = _context.Users.Where(u => selectedPatients.Contains(u.Id)).ToList();

        foreach (var patient in patientsToEmail)
        {
            await _emailSender.SendEmailAsync(patient.Email, subject, message);
        }

        // Redirect to the same page with a success message or similar.
        return RedirectToAction("ListPatients", new { success = "Emails sent successfully." });
    }

}
