using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using local_roots.Models;
namespace local_roots.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context,  ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        //get role
        string role = HttpContext.Session.GetString("Role");
        string userId = HttpContext.Session.GetString("UserId");
        switch (role)
        {
            case "Customer":
                 return RedirectToAction("All", "Product");


            case "Vendor":
              return RedirectToAction("VendorDashboard", "Vendor", new { id = userId });



            case "Admin":
               return RedirectToAction("Dashboard", "Admin", new { id = userId });

            default:
                 return RedirectToAction("Continue", "Home");
        }
    }
 [HttpGet]
    public IActionResult Continue()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ContinueAs(string email, string role)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(role))
        {
            TempData["Error"] = "Email and role selection are required.";
            return RedirectToAction("Continue");
        }

        if (role == "Customer")
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Email == email);
            if (customer == null)
            {
                customer = new Customer
                {
                    Email = email,
                    FullName = "New Customer", // default name
                    PasswordHash = "", // optional default or empty
                                       // add other fields if required
                };
                _context.Customers.Add(customer);
                _context.SaveChanges();
            }

            HttpContext.Session.SetString("UserId", customer.UserId.ToString());
            HttpContext.Session.SetString("Role", "Customer");
            return RedirectToAction("All", "Product");
        }
        else if (role == "Vendor")
        {
            var seller = _context.Sellers.FirstOrDefault(s => s.Email == email);
            if (seller == null)
            {
                seller = new Seller
                {
                    Email = email,
                    StoreName = "New Store", // default name
                    PasswordHash = "", // optional
                };
                _context.Sellers.Add(seller);
                _context.SaveChanges();
            }


            HttpContext.Session.SetString("UserId", seller.UserId.ToString());
            HttpContext.Session.SetString("Role", "Vendor");

            return RedirectToAction("VendorDashboard", "Vendor", new { id = seller.UserId });
        }
        else if (role == "Admin")
        {
            var admin = _context.Admins.FirstOrDefault(a => a.Email == email);
            if (admin == null)
            {
                TempData["Error"] = "Admin account not found.";
                return RedirectToAction("Continue");
            }

            HttpContext.Session.SetString("UserId", admin.UserId.ToString());
            HttpContext.Session.SetString("Role", "Admin");

            return RedirectToAction("Dashboard", "Admin", new { id = admin.UserId });
        }
        TempData["Error"] = "Invalid role selection.";
        return RedirectToAction("Continue");
    }

    public IActionResult Logout()
    {
        //get role
        string role = HttpContext.Session.GetString("Role");
        switch (role)
        {
            case "Customer":
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("Role");
                break;


            case "Vendor":
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("Role");
                break;


            case "Admin":
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("Role");
                break;

            default:
                TempData["Error"] = "You are not logged in.";
                break;
        }

        return RedirectToAction("Continue", "Home");
    }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
