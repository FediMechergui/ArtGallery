using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [Required]
        public required string UserId { get; set; }

        [Required]
        public required ApplicationUser User { get; set; }

        [Required]
        public int ArtworkId { get; set; }

        [Required]
        public required Artwork Artwork { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
} 