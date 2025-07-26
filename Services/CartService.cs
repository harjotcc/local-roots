using local_roots.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace local_roots.Services
{
  public interface ICartService
{
    List<CartItem> GetCartItems();
    void AddToCart(Product product);
    void RemoveFromCart(int productId);
    void ClearCart();
    void ChangeQuantity(int productId, int delta); 
}

   public class CartService : ICartService
{
    private const string CartSessionKey = "Cart";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public List<CartItem> GetCartItems()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var json = session.GetString(CartSessionKey);

        return string.IsNullOrEmpty(json)
            ? new List<CartItem>()
            : JsonConvert.DeserializeObject<List<CartItem>>(json);
    }

    public void AddToCart(Product product)
    {
        var cart = GetCartItems();
        var item = cart.FirstOrDefault(c => c.Product.Id == product.Id);
        if (item != null)
        {
            item.Quantity = Math.Min(item.Quantity + 1, 10); // Max 10
        }
        else
        {
            cart.Add(new CartItem { Product = product, Quantity = 1 });
        }
        Console.WriteLine($">>>>>Adding {product.Name} to cart. Current quantity: {cart.FirstOrDefault(c => c.Product.Id == product.Id)?.Quantity ?? 0}");

        SaveCartItems(cart);
    }

    public void RemoveFromCart(int productId)
    {
        var cart = GetCartItems();
        var item = cart.FirstOrDefault(c => c.Product.Id == productId);
        if (item != null)
        {
            cart.Remove(item);
            SaveCartItems(cart);
        }
    }

    public void ClearCart()
    {
        SaveCartItems(new List<CartItem>());
    }

    public void ChangeQuantity(int productId, int delta)
    {
        var cart = GetCartItems();
        var item = cart.FirstOrDefault(c => c.Product.Id == productId);
        if (item != null)
        {
            item.Quantity += delta;
            item.Quantity = Math.Clamp(item.Quantity, 1, 10); // Between 1 and 10
            SaveCartItems(cart);
        }
    }

    private void SaveCartItems(List<CartItem> cart)
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var json = JsonConvert.SerializeObject(cart);
        session.SetString(CartSessionKey, json);
    }
}

}
