using Microsoft.AspNetCore.Mvc;
using local_roots.Models;
using System.Collections.Generic;
using System.Linq;

namespace local_roots.Controllers
{
    public class VendorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult VendorDashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
               var product = new Product
               {
                    VendorId = "test",
                    Rating = 0, // Default rating
                    NumRaters = 0, // Default number of raters
            };
            return View(product);
        }

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddProduct(Product product)
{
    if (ModelState.IsValid)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home"); // or another page
    }
    else Console.WriteLine("Model state is invalid. Errors: " + string.Join(", ", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))));

    return View("AddProduct", product); // Ensure it renders the correct view
}


        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return RedirectToAction("MyProducts");
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id, string confirm)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("MyProducts");
        }

        [HttpGet]
        public IActionResult MyProducts()
        {
            // For demo: fetch all products. Filter by vendor if you have VendorId.
            var products = _context.Products.ToList();
            return View(products);
        }
    }
}
