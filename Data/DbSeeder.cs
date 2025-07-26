using local_roots.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        try
        {
            if (context.Customers.Any() || context.Sellers.Any() || context.Products.Any())
                return;

            // Resolve correct path to JSON file
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var jsonPath = Path.Combine(basePath, "Data", "seed_data.json");
            Console.WriteLine($"Seeding data from: {jsonPath}");

            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"Seed file not found at: {jsonPath}");
                return;
            }

            var jsonData = await File.ReadAllTextAsync(jsonPath);

            var seedData = JsonConvert.DeserializeObject<SeedData>(jsonData);
            if (seedData == null)
            {
                Console.WriteLine("Failed to deserialize seed data.");
                return;
            }

            await context.Customers.AddRangeAsync(seedData.Customers);
            await context.Sellers.AddRangeAsync(seedData.Sellers);
            await context.Products.AddRangeAsync(seedData.Products);
            await context.Orders.AddRangeAsync(seedData.Orders);
            await context.OrderItems.AddRangeAsync(seedData.OrderItems);
            await context.Reviews.AddRangeAsync(seedData.Reviews);

            await context.SaveChangesAsync();
            Console.WriteLine("Database seeded successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Seeding failed: {ex.Message}");
        }
    }
    public class SeedData
    {
        public List<Customer> Customers { get; set; }
        public List<Seller> Sellers { get; set; }
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Review> Reviews { get; set; }
    }
}