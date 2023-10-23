using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasyImagery.Data;
using EasyImagery.Models;
using EasyImagery.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CsvHelper;
using OfficeOpenXml;
using System.Globalization;

namespace EasyImagery
{
    [Authorize]
    public class TimeslotsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public TimeslotsController(ApplicationDbContext context, EmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        // GET: Timeslots
        public async Task<IActionResult> Index(string? pid)
        {
            string physicianId = "1";
            if (pid != null)
            {
                physicianId = pid;
            }
            var applicationDbContext = _context.Timeslot.Where(t => t.PhysicianId == pid).Include(t => t.Physician);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> TimeslotsByClinic(int clinicId, int pageNumber = 1, string searchTerm = null, string sortOrder = "asc")
        {
            const int PageSize = 10;

            var physicianIds = await _context.Physician
                .Where(p => p.PhysicianClinicId == clinicId)
                .Select(p => p.Id)
                .ToListAsync();

            var query = _context.Timeslot
                .Where(t => physicianIds.Contains(t.PhysicianId));

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.Description.Contains(searchTerm));
            }

            switch (sortOrder.ToLower())
            {
                case "desc":
                    query = query.OrderByDescending(t => t.StartDate);
                    break;
                default:
                    query = query.OrderBy(t => t.StartDate);
                    break;
            }

            var timeslots = await query
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var totalItems = await query.CountAsync();

            var model = new TimeslotViewModel
            {
                Timeslots = timeslots,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize),
                SearchTerm = searchTerm,
                ClinicId = clinicId,
                SortOrder = sortOrder
            };

            return View(model);
        }

        // GET: Timeslots/MyTimeslots
        [Authorize(Roles = "Physician")] // Assuming physicians have a role named "Physician"
        public async Task<IActionResult> MyTimeslots()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = "User not authorized." });
            }

            var timeslots = await _context.Timeslot
                                         .Where(t => t.PhysicianId == user.Id)
                                         .Include(t => t.Patient)
                                         .ToListAsync();
            return View(timeslots);
        }

        // GET: Timeslots/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Timeslot == null)
            {
                return NotFound();
            }

            var timeslot = await _context.Timeslot
                .Include(t => t.Physician)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeslot == null)
            {
                return NotFound();
            }

            return View(timeslot);
        }

        public async Task<IActionResult> BookTimeslot(long timeslotId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = "User not authorized." });
            }

            var existingBooking = await _context.Timeslot.FirstOrDefaultAsync(t => t.PatientId == user.Id);
            if (existingBooking != null)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = "You've already booked an appointment. You can only book one timeslot at a time." });
            }

            var timeslot = await _context.Timeslot.FindAsync(timeslotId);
            if (timeslot == null)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = "Timeslot not found." });
            }
            if (timeslot.PatientId != null)
            {
                return RedirectToAction("Error", "Home", new { errorMessage = "This timeslot is already booked by another user." });
            }

            timeslot.PatientId = user.Id;
            _context.Timeslot.Update(timeslot);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { pid = timeslot.PhysicianId });
        }

        // GET: Timeslots/Create
        public IActionResult Create(string physicianId)
        {
            ViewData["PhysicianId"] = physicianId != null? physicianId : new SelectList(_context.Physician, "Id", "Id");
            return View();
        }

        // POST: Timeslots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,StartDate,EndDate,PhysicianId,PatientId,Rating,ImageData")] Timeslot timeslot, IFormFile Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await Image.CopyToAsync(memoryStream);
                    timeslot.ImageData = memoryStream.ToArray();
                }
                _context.Add(timeslot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PhysicianId"] = new SelectList(_context.Physician, "Id", "Id", timeslot.PhysicianId);
            return View(timeslot);
        }

        // GET: Timeslots/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Timeslot == null)
            {
                return NotFound();
            }

            var timeslot = await _context.Timeslot.FindAsync(id);
            if (timeslot == null)
            {
                return NotFound();
            }
            ViewData["PhysicianId"] = new SelectList(_context.Physician, "Id", "Id", timeslot.PhysicianId);
            return View(timeslot);
        }

        // POST: Timeslots/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Description,StartDate,EndDate,PhysicianId,PatientId,Rating,ImageData")] Timeslot timeslot, IFormFile Image)
        {
            if (id != timeslot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (Image != null && Image.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await Image.CopyToAsync(memoryStream);
                    timeslot.ImageData = memoryStream.ToArray();
                }
                try
                {
                    _context.Update(timeslot);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TimeslotExists(timeslot.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PhysicianId"] = new SelectList(_context.Physician, "Id", "Id", timeslot.PhysicianId);
            return View(timeslot);
        }

        // GET: Timeslots/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Timeslot == null)
            {
                return NotFound();
            }

            var timeslot = await _context.Timeslot
                .Include(t => t.Physician)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (timeslot == null)
            {
                return NotFound();
            }

            return View(timeslot);
        }

        // POST: Timeslots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Timeslot == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Timeslot'  is null.");
            }
            var timeslot = await _context.Timeslot.FindAsync(id);
            if (timeslot != null)
            {
                _context.Timeslot.Remove(timeslot);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TimeslotExists(long id)
        {
          return (_context.Timeslot?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<IActionResult> SendImageToPatient(long id)
        {
            var timeslot = await _context.Timeslot
                .Include(t => t.Patient)
                .FirstOrDefaultAsync(t => t.Id == id);

            var patientEmail = timeslot.Patient.Email;
            var subject = "Your Medical Image";
            var body = "Here is the Medical Image.";

            await _emailSender.SendEmailWithAttachmentAsync(patientEmail, subject, body, timeslot.ImageData, "image.jpg");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportTimeslots(string physicianId, string format)
        {
            var timeslots = await _context.Timeslot
                .Where(t => t.PhysicianId == physicianId)
                .ToListAsync();

            var projectedTimeslots = timeslots.Select(t => new
            {
                t.Id,
                t.Description,
                t.StartDate,
                t.EndDate,
                t.PhysicianId,
                t.PatientId,
                t.Rating
            }).ToList();

            var memoryStream = new MemoryStream();

            if (format == "csv")
            {
                using var writer = new StreamWriter(memoryStream, leaveOpen: true);
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(projectedTimeslots);
                await writer.FlushAsync();
                memoryStream.Position = 0;

                return File(memoryStream, "text/csv", "Timeslots.csv");
            }
            else if (format == "xlsx")
            {
                using var package = new ExcelPackage(memoryStream);
                var worksheet = package.Workbook.Worksheets.Add("Timeslots");
                worksheet.Cells.LoadFromCollection(projectedTimeslots, true);
                package.Save();
                memoryStream.Position = 0;

                return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Timeslots.xlsx");
            }

            return RedirectToAction("Error", "Home", new { errorMessage = "Invaild Format" });
        }

    }
}
