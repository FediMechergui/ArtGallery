using Microsoft.AspNetCore.Identity;

namespace ArtGallery.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }

        public required ICollection<Order> Orders { get; set; } = new List<Order>();
        public required ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
    }
} 