using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Models
{
    // Représente une œuvre d'art avec ses détails : titre, description, technique, prix, catégories, images et états.
    public class Artwork
    {
        // Initialise une nouvelle instance de la classe Artwork et les collections associées.
        public Artwork()
        {
            Categories = new List<Category>();
            ExhibitionArtworks = new List<ExhibitionArtwork>();
            ShoppingCarts = new List<ShoppingCart>();
            OrderDetails = new List<OrderDetail>();
            Images = new List<ArtworkImage>();
        }

        // Identifiant unique de l'œuvre.
        public int Id { get; set; }

        // Titre de l'œuvre.
        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        // Description de l'œuvre.
        [Required]
        public required string Description { get; set; }

        // Technique utilisée pour réaliser l'œuvre.
        [Required]
        public required string TechniqueUsed { get; set; }

        // Dimensions de l'œuvre.
        [Required]
        public required string Size { get; set; }

        // Prix de l'œuvre.
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Date de création de l'œuvre.
        [Required]
        public DateTime CreationDate { get; set; }

        // Date et heure d'ajout de l'œuvre dans la base de données.
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Date et heure de la dernière modification de l'œuvre.
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Indique si l'œuvre est disponible.
        public bool IsAvailable { get; set; } = true;

        // Indique si l'œuvre est en vente.
        public bool IsForSale { get; set; } = true;

        // Indique si l'œuvre est en avant (mise en avant).
        public bool IsFeatured { get; set; } = false;

        // Nombre de fois que l'œuvre a été consultée.
        public int ViewCount { get; set; }

        // Catégories associées à l'œuvre.
        public required ICollection<Category> Categories { get; set; }

        // Relations exposition-œuvre pour cette œuvre.
        public required ICollection<ExhibitionArtwork> ExhibitionArtworks { get; set; }

        // Paniers contenant cette œuvre.
        public required ICollection<ShoppingCart> ShoppingCarts { get; set; }

        // Détails de commande pour cette œuvre.
        public required ICollection<OrderDetail> OrderDetails { get; set; }

        // Images associées à l'œuvre.
        public required ICollection<ArtworkImage> Images { get; set; }
    }
}