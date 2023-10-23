using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EasyImagery.Data; // Replace with your actual namespace containing your DbContext
using System.Linq;
using System.Threading.Tasks;
using EasyImagery.Data;
using EasyImagery.Models;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class AppointmentsController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AppointmentsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> MyAppointments()
    {
        var userId = _userManager.GetUserId(User);
        var appointments = await _context.Timeslot
            .Where(t => t.PatientId == userId)
            .Include(t => t.Physician)
            .ToListAsync();
        return View(appointments);
    }

    [HttpPost]
    public async Task<IActionResult> CancelAppointment(long id)
    {
        var timeslot = await _context.Timeslot.FindAsync(id);
        if (timeslot == null || timeslot.PatientId != _userManager.GetUserId(User))
        {
            return NotFound();
        }
        timeslot.PatientId = null; // Unassign the patient from the timeslot
        _context.Update(timeslot);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(MyAppointments));
    }

    [HttpPost]
    [Authorize(Roles = "Physician")]
    public async Task<IActionResult> CancelAppointmentFromPhysician(long timeslotId)
    {
        var timeslot = await _context.Timeslot.FindAsync(timeslotId);
        if (timeslot == null)
        {
            return RedirectToAction("Error", "Home", new { errorMessage = "Timeslot not found." });
        }
        if (timeslot.PhysicianId != _userManager.GetUserId(User))
        {
            return RedirectToAction("Error", "Home", new { errorMessage = "You can't cancel this appointment." });
        }

        timeslot.PatientId = null;
        _context.Timeslot.Update(timeslot);
        await _context.SaveChangesAsync();

        return RedirectToAction("MyTimeslots", "Timeslots");
    }
}