using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using local_roots.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace local_roots.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Dashboard/{id}
        public async Task<IActionResult> Dashboard(int id)
        {
            // Validate session user and role
            var sessionUserId = HttpContext.Session.GetString("UserId");
            var sessionRole = HttpContext.Session.GetString("Role");

            if (sessionUserId == null || sessionRole != "Admin" || sessionUserId != id.ToString())
            {
                // Unauthorized, redirect to login or Continue page
                return RedirectToAction("Continue", "Home");
            }

            // Find admin by id
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            // Load all related data for admin dashboard view
            ViewBag.Customers = await _context.Customers.ToListAsync();
            ViewBag.Sellers = await _context.Sellers.ToListAsync();
            ViewBag.Orders = await _context.Orders.ToListAsync();
            ViewBag.Reviews = await _context.Reviews.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();

            // Pass admin object to view
            return View(admin);
        }
    }
}
