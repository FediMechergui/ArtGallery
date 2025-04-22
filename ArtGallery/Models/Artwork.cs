using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Models
{
    public class Artwork
    {
        public Artwork()
        {
            Categories = new List<Category>();
            ExhibitionArtworks = new List<ExhibitionArtwork>();
            ShoppingCarts = new List<ShoppingCart>();
            OrderDetails = new List<OrderDetail>();
            Images = new List<ArtworkImage>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required string TechniqueUsed { get; set; }


        [Required]
        public required string Size { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsAvailable { get; set; } = true;
        public bool IsForSale { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        public int ViewCount { get; set; }

        public required ICollection<Category> Categories { get; set; }
        public required ICollection<ExhibitionArtwork> ExhibitionArtworks { get; set; }
        public required ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public required ICollection<OrderDetail> OrderDetails { get; set; }
        public required ICollection<ArtworkImage> Images { get; set; }
    }
} 