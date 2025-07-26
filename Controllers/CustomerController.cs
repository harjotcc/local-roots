using Microsoft.AspNetCore.Mvc;
using local_roots.Models; 
using local_roots.Services; 
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class CustomerController : Controller
{
    private readonly ICartService _cartManager;
    private readonly IProductService _productService;
    private readonly ApplicationDbContext _context;
    private readonly IOrderService _orderService;
    private readonly IReviewService _reviewService;

    public CustomerController(ApplicationDbContext context, ICartService cartManager, IProductService productService, IOrderService orderService ,IReviewService reviewService)
    {
        _cartManager = cartManager;
        _productService = productService;
        _context = context;
        _orderService = orderService;
        _reviewService = reviewService;
    }

    [HttpGet]
    public IActionResult Cart()
    {
        var cartItems = _cartManager.GetCartItems();

        Console.WriteLine(">>>>>> Cart items retrieved: " + cartItems.Count);

        return View(cartItems);
    }

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

    [HttpPost]
    public IActionResult RemoveFromCart(int id)
    {
        _cartManager.RemoveFromCart(id);
        return RedirectToAction("Cart");
    }

    [HttpPost]
    public IActionResult IncreaseQuantity(int id)
    {
        _cartManager.ChangeQuantity(id, +1);
        return RedirectToAction("Cart");
    }

    [HttpPost]
    public IActionResult DecreaseQuantity(int id)
    {
        _cartManager.ChangeQuantity(id, -1);
        return RedirectToAction("Cart");
    }

    [HttpPost]
    public IActionResult BuyAll()
    {
        var cartItems = _cartManager.GetCartItems();
        if (cartItems == null || !cartItems.Any())
        {
            TempData["Error"] = "Your cart is empty.";
            return RedirectToAction("Cart");
        }

        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            TempData["Error"] = "User not logged in.";
            return RedirectToAction("Login", "Home");
        }

        var order = new Order
        {
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            CustomerId = int.Parse(userId),
            TotalPrice = cartItems.Sum(i => i.Product.Price * i.Quantity),
            OrderItems = new List<OrderItem>()
        };

        foreach (var item in cartItems)
        {
            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.Product.Id,
                Quantity = item.Quantity,
                Price = item.Product.Price
            });
        }

        _context.Orders.Add(order);
        _context.SaveChanges();

        _cartManager.ClearCart();

        return RedirectToAction("Receipt", new { id = order.OrderId });
    }
    public IActionResult Receipt(int id)
    {
        var order = _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .FirstOrDefault(o => o.OrderId == id);

        if (order == null)
            return NotFound();

        return View(order);
    }
    // GET: /Customer/Orders
    public async Task<IActionResult> Orders()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        {
            // Optionally redirect to login or show an error
            return RedirectToAction("Login", "Account");
        }

        var orders = await _orderService.GetOrdersByCustomerIdAsync(userId);
        return View(orders);
    }
    [HttpPost]
    public IActionResult AddReview(Review review)
    {
        //get customer id from session
        var userIdStr = HttpContext.Session.GetString("UserId");

        review.CustomerId = string.IsNullOrEmpty(userIdStr) ? 0 : int.Parse(userIdStr);


        review.Date = DateTime.Now;
        _reviewService.AddReview(review);
        return RedirectToAction("Details", "Product", new { id = review.ProductId });

        
   
   
    }


}