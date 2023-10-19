using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EasyImagery.Data; // Replace with your actual namespace
using System.Linq;
using System.Threading.Tasks;

public class PhysicianController : Controller
{
    private readonly ApplicationDbContext _context; // Replace with your DbContext name

    public PhysicianController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Physicians
    public async Task<IActionResult> Index()
    {
        var physicians = await _context.Users
            .Where(u => u.UserType == "Physician")
            .ToListAsync();

        return View(physicians);
    }
}
