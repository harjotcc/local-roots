namespace local_roots.Models;


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


    public abstract class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        // EF adds the discriminator column automatically (UserType)
    }

    public class Customer : User
    {
        public string FullName { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }

    public class Seller : User
    {
        public string StoreName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
    public class Admin : User
    {
        public string Name { get; set; }  
    }

    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }

        public int SellerId { get; set; }
        public Seller Seller { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }

    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }

public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }
}
