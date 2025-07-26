// Services/OrderService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using local_roots.Models;
using Microsoft.EntityFrameworkCore;


public interface IOrderService
{
    Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
    Task<Order> CreateOrderAsync(int customerId, List<(int productId, int quantity)> cartItems);
}

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

 public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
{
    return await _context.Orders
        .Where(o => o.CustomerId == customerId)
        .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
        .ToListAsync();
}


    public async Task<Order> CreateOrderAsync(int customerId, List<(int productId, int quantity)> cartItems)
    {
        var products = await _context.Products
            .Where(p => cartItems.Select(c => c.productId).Contains(p.Id))
            .ToListAsync();

        var orderItems = new List<OrderItem>();
        decimal total = 0;

        foreach (var (productId, quantity) in cartItems)
        {
            var product = products.FirstOrDefault(p => p.Id == productId);
            if (product == null) continue;

            var itemTotal = product.Price * quantity;
            total += itemTotal;

            orderItems.Add(new OrderItem
            {
                ProductId = productId,
                Quantity = quantity,
                Price = itemTotal
            });
        }

        var order = new Order
        {
            CustomerId = customerId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            TotalPrice = total,
            OrderItems = orderItems
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order;
    }
}
