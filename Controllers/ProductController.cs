using Microsoft.AspNetCore.Mvc;
using local_roots.Models;

namespace local_roots.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController()
        {
            _productService = new ProductService(); // Manual injection for demonstration
        }

        // Show details for a single product
        public IActionResult Details(int id)
        {
            var products = _productService.GetAllProducts();
            if (id < 0 || id >= products.Count)
            {
                return NotFound();
            }
            var product = products[id];
            return View(product);
        }
        // GET: /Product/All
        public IActionResult All()
        {
            var products = _productService.GetAllProducts();
            return View(products);
        }
    }
}