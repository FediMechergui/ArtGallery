using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    // Représente une entrée dans le livre d'or (guestbook) de la galerie.
    public class GoldenBookEntry
    {
        // Identifiant unique de l'entrée.
        public int Id { get; set; }

        // Nom du visiteur.
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        // Adresse email du visiteur.
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        // Message laissé par le visiteur.
        [Required]
        public required string Message { get; set; }

        // Date de création de l'entrée.
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // Indique si l'entrée a été approuvée.
        public bool IsApproved { get; set; }
    }
}