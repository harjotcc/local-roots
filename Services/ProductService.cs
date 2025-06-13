using System.Collections.Generic;
using System.Linq;
using local_roots.Models;

public interface IProductService
{
    List<Product> GetAllProducts();
    Product GetProductById(int id);
}

public class ProductService : IProductService
{
    private static List<Product> _products = new List<Product>
    {
        new Product
        {
            Id = 1,
            Name = "Fresh Apples",
            Price = "$2.99",
            Description = "Crisp, juicy apples picked fresh from local orchards. Perfect for snacking or baking your favorite pie recipes.",
            ImageUrl = "/images/def-product.png",
            Rating = 4.7f,
            NumRaters = 120,
            VendorId = "V001"
        },
        new Product
        {
            Id = 2,
            Name = "Organic Bananas",
            Price = "$1.99",
            Description = "Sweet, organic bananas grown without pesticides. Great for smoothies, cereal, or a healthy snack on the go.",
            ImageUrl = "/images/def-product.png",
            Rating = 4.5f,
            NumRaters = 98,
            VendorId = "V002"
        },
        new Product
        {
            Id = 3,
            Name = "Local Honey",
            Price = "$5.49",
            Description = "Pure, raw honey harvested from local bees. Add natural sweetness to your tea, toast, or desserts.",
            ImageUrl = "/images/def-product.png",
            Rating = 4.8f,
            NumRaters = 75,
            VendorId = "V003"
        },
        new Product
        {
            Id = 4,
            Name = "Farm Eggs",
            Price = "$3.99",
            Description = "Fresh eggs from free-range hens. Rich in flavor and nutrients, ideal for breakfast or baking.",
            ImageUrl = "/images/def-product.png",
            Rating = 4.6f,
            NumRaters = 110,
            VendorId = "V004"
        },
        new Product
        {
            Id = 5,
            Name = "Almond Milk",
            Price = "$4.29",
            Description = "Creamy almond milk made from the finest almonds. Dairy-free and perfect for coffee, cereal, or recipes.",
            ImageUrl = "/images/def-product.png",
            Rating = 4.4f,
            NumRaters = 60,
            VendorId = "V005"
        },
        new Product
        {
            Id = 6,
            Name = "Fresh Bread",
            Price = "$2.49",
            Description = "Soft, artisan bread baked daily. Enjoy with butter, as a sandwich, or alongside your favorite soup.",
            ImageUrl = "/images/def-product.png",
            Rating = 4.9f,
            NumRaters = 150,
            VendorId = "V006"
        },
        new Product
        {
            Id = 7,
            Name = "Tomato Sauce",
            Price = "$3.59",
            Description = "Rich and tangy tomato sauce made from locally grown tomatoes. Perfect for pasta and pizza.",
            ImageUrl = "/images/def-product.png",
            Rating = 4.3f,
            NumRaters = 80,
            VendorId = "V007"
        },
        new Product
        {
            Id = 8,
            Name = "Cheddar Cheese",
            Price = "$5.99",
            Description = "Aged cheddar cheese with a sharp, creamy flavor. Great for sandwiches, burgers, or snacking.",
            ImageUrl = "/images/def-product.png",
            Rating = 4.8f,
            NumRaters = 105,
            VendorId = "V008"
        }
    };

    public List<Product> GetAllProducts()
    {
        return _products;
    }

    public Product GetProductById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }
}