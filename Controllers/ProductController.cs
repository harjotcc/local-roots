using Microsoft.AspNetCore.Mvc;
using local_roots.Models;
using local_roots.Services;
using Microsoft.EntityFrameworkCore;

namespace local_roots.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartManager;
        private readonly ApplicationDbContext _context;
        private readonly IReviewService _reviewService;

        public ProductController(IProductService productService, ICartService cartManager, ApplicationDbContext context,IReviewService reviewService)
        {
            _productService = productService;
            _cartManager = cartManager;
            _context = context;
            _reviewService = reviewService;
        }

        public IActionResult All()
        {
            ViewBag.Role = HttpContext.Session.GetString("Role");
            var products = _productService.GetAllProducts();

            foreach (var product in products)
            {
                // Load seller information for each product
                product.Reviews = _reviewService.GetReviewsByProductId(product.Id);
            }



            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();

            
            product.Reviews = _reviewService.GetReviewsByProductId(id);


            return View(product);
        }

        //just for testing
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var product = _productService.GetProductById(id);

            Console.WriteLine($">>>>> Attempting to add product with ID {id} to cart.");

            if (product != null)
            {
                _cartManager.AddToCart(product);
            }
            return RedirectToAction("All", "Product");
        }

     public IActionResult Search(string q, string type)
    {
    Console.WriteLine($">>>>> Search query: {q}, type: {type}");

        return View();
    }

        //Reviews 
        public IActionResult Review(int id)
        {
            var reviews = _reviewService.GetReviewsByProductId(id);
            ViewBag.ProductId = id;
            return View(reviews);
        }

    }
}