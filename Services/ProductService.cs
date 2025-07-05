using System.Collections.Generic;
using System.Linq;
using local_roots.Models;
using Microsoft.EntityFrameworkCore;

public interface IProductService
{
    List<Product> GetAllProducts();
    Product GetProductById(int id);
}

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Product> GetAllProducts()
    {
        return _context.Products.ToList();
    }

    public Product GetProductById(int id)
    {
        return _context.Products.FirstOrDefault(p => p.Id == id);
    }
}