using Microsoft.AspNetCore.Mvc;
using local_roots.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using local_roots.Services;

namespace local_roots.Controllers
{
    public class VendorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dashboard for vendor - customize as needed
        public IActionResult VendorDashboard(int id)
        {
            var vendor = _context.Sellers.FirstOrDefault(v => v.UserId == id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }

        // GET: Show form to add a new product
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        // POST: Add new product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            product.SellerId = GetCurrentSellerId(); // Set SellerId from session

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyProducts");
        }

        private int GetCurrentSellerId()
        {
            //get user id from session
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                // Handle error - user not logged in or invalid session
                Console.WriteLine(">>>>>> Invalid UserId in session");
                return 0; // or throw an exception
            }
            return userId;

        }

        // GET: Edit product form
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            // Optional: Verify product belongs to logged-in vendor before allowing edit

            return View(product);
        }

        // POST: Update product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("MyProducts");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(product);
        }

        // GET: Confirm delete product
        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            // Optional: Verify product ownership here

            return View(product);
        }

        // POST: Delete product action
        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                // Optional: Verify product ownership before deleting

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("MyProducts");
        }

        // List all products for the logged-in vendor
        [HttpGet]
        public async Task<IActionResult> MyProducts()
        {
            // Get UserId from session
            var userIdString = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                TempData["Error"] = "You must be logged in to view your products.";
                return RedirectToAction("Continue", "Home"); // or your login page
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                TempData["Error"] = "Invalid session UserId.";
                return RedirectToAction("Continue", "Home");
            }

            // Get products where SellerId matches logged-in user
            var products = await _context.Products
                .Where(p => p.SellerId == userId)
                .ToListAsync();

            return View(products);
        }


        // View product details
        [HttpGet]
        public async Task<IActionResult> ViewProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Seller)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

       

    }
}
