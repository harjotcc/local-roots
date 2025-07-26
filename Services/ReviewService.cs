using local_roots.Models;
using Microsoft.EntityFrameworkCore;
namespace local_roots.Services
{

    public interface IReviewService
    {
        void AddReview(Review review);
        List<Review> GetReviewsByProductId(int productId);
    }
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();


            Console.WriteLine($">>>>> Review added for Product ID: {review.ProductId}, Customer ID: {review.CustomerId}, Rating: {review.Rating}");
        }

        public List<Review> GetReviewsByProductId(int productId)
        {
            return _context.Reviews
                .Include(r => r.Customer)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.Date)
                .ToList();
        }
    }
}
