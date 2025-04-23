using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Models
{
    // Représente un panier d'achat d'un utilisateur.
    public class ShoppingCart
    {
        // Identifiant unique du panier.
        public int Id { get; set; }

        // Identifiant de l'utilisateur propriétaire du panier.
        [Required]
        public required string UserId { get; set; }

        // Référence à l'utilisateur propriétaire du panier.
        [Required]
        public required ApplicationUser User { get; set; }

        // Identifiant de l'œuvre dans le panier.
        [Required]
        public int ArtworkId { get; set; }

        // Référence à l'œuvre dans le panier.
        [Required]
        public required Artwork Artwork { get; set; }

        // Quantité de cette œuvre dans le panier.
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        // Prix unitaire de l'œuvre dans le panier.
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Sous-total pour cette ligne du panier.
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        // Date d'ajout de l'œuvre dans le panier.
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}