using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    // Représente l'association entre une exposition et une œuvre d'art.
    public class ExhibitionArtwork
    {
        // Identifiant de l'exposition associée.
        [Required]
        public int ExhibitionId { get; set; }
        // Référence à l'exposition.
        public required Exhibition Exhibition { get; set; }

        // Identifiant de l'œuvre associée.
        [Required]
        public int ArtworkId { get; set; }
        // Référence à l'œuvre.
        public required Artwork Artwork { get; set; }

        // Ordre d'affichage de l'œuvre dans l'exposition.
        public int DisplayOrder { get; set; }
        // Indique si l'œuvre est mise en avant dans l'exposition.
        public bool IsFeatured { get; set; }
    }
}