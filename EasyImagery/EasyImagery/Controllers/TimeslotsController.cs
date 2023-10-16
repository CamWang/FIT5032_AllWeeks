using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EasyImagery.Data;
using EasyImagery.Models;

namespace EasyImagery
{
    public class TimeslotsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TimeslotsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Timeslots
        public async Task<IActionResult> Index(string? itemId)
        {
            string physicianId = "1";
            if (itemId != null)
            {
                physicianId = itemId;
            }
            var applicationDbContext = _context.Timeslot.Where(t => t.PhysicianId == itemId).Include(t => t.Physician);
            return View(await applicationDbContext.ToListAsync());
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

        // GET: Timeslots/Create
        public IActionResult Create()
        {
            ViewData["PhysicianId"] = new SelectList(_context.Physician, "Id", "Id");
            return View();
        }

        // POST: Timeslots/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,StartDate,EndDate,PhysicianId,PatientId,Rating,ImageData")] Timeslot timeslot)
        {
            if (ModelState.IsValid)
            {
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
        public async Task<IActionResult> Edit(long id, [Bind("Id,Description,StartDate,EndDate,PhysicianId,PatientId,Rating,ImageData")] Timeslot timeslot)
        {
            if (id != timeslot.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
    }
}
