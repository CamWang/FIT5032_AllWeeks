using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EasyImagery.Data; // Replace with your actual namespace
using EasyImagery.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EasyImagery.Controllers
{
    public class PhysicianController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Physician> _userManager;

        public PhysicianController(ApplicationDbContext context, UserManager<Physician> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? itemId)
        {
            int clinicId = 1;
            if (itemId != null)
            {
                clinicId = itemId.Value;
            }
            var physicians = await _userManager.Users
                                       .Where(p => p.ClinicId == clinicId)
                                       .ToListAsync();
            return View(physicians);
        }
    }
}
