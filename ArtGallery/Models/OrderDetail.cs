using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Models
{
    // Représente un détail d'une commande (ligne de commande).
    public class OrderDetail
    {
        // Identifiant unique du détail de commande.
        public int Id { get; set; }

        // Identifiant de la commande associée.
        [Required]
        public int OrderId { get; set; }

        // Référence à la commande associée.
        [Required]
        public required Order Order { get; set; }

        // Identifiant de l'œuvre commandée.
        [Required]
        public int ArtworkId { get; set; }

        // Référence à l'œuvre commandée.
        [Required]
        public required Artwork Artwork { get; set; }

        // Prix unitaire de l'œuvre au moment de la commande.
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Quantité commandée.
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        // Sous-total pour cette ligne de commande.
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }
    }
}