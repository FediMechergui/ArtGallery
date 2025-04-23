using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    // Représente une image associée à une œuvre d'art.
    public class ArtworkImage
    {
        // Identifiant unique de l'image.
        public int Id { get; set; }

        // Identifiant de l'œuvre associée.
        [Required]
        public int ArtworkId { get; set; }
        // Référence à l'œuvre associée.
        public required Artwork Artwork { get; set; }

        // Chemin du fichier image.
        [Required]
        [StringLength(500)]
        public required string ImagePath { get; set; }

        // Indique si cette image est l'image principale de l'œuvre.
        public bool IsPrimary { get; set; }
        // Ordre d'affichage de l'image.
        public int DisplayOrder { get; set; }
        // Date d'ajout de l'image.
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}