using Microsoft.AspNetCore.Mvc;
using local_roots.Models;

namespace local_roots.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        // Use dependency injection for IProductService
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // Show details for a single product
        public IActionResult Details(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
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