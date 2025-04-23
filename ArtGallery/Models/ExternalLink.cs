using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    // Représente un lien externe associé à la galerie.
    public class ExternalLink
    {
        // Identifiant unique du lien externe.
        public int Id { get; set; }

        // Titre du lien externe.
        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        // URL du lien externe.
        [Required]
        [Url]
        [StringLength(500)]
        public required string Url { get; set; }

        // Description du lien externe.
        [Required]
        [StringLength(500)]
        public required string Description { get; set; }

        // Type du lien externe.
        [Required]
        [StringLength(50)]
        public required string Type { get; set; }

        // Indique si le lien est actif.
        public bool IsActive { get; set; } = true;
        // Ordre d'affichage du lien.
        public int SortOrder { get; set; } = 0;
        // Date de création du lien.
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // Date de dernière modification du lien (optionnel).
        public DateTime? UpdatedAt { get; set; }
    }
}