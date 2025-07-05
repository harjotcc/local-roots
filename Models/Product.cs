namespace local_roots.Models;
public class Product
{
    public int Id { get; set; } // Added Id property
    public string Name { get; set; }
    public string Price { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public float Rating { get; set; } // Changed to float
    public int NumRaters { get; set; }
    public int Count { get; set; } 
    public string VendorId { get; set; }
}