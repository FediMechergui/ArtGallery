using System;
using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    // Représente une exposition d'œuvres d'art.
    public class Exhibition
    {
        // Initialise une nouvelle instance de la classe Exhibition et la collection d'œuvres exposées.
        public Exhibition()
        {
            ExhibitionArtworks = new List<ExhibitionArtwork>();
        }

        // Identifiant unique de l'exposition.
        public int Id { get; set; }

        // Titre de l'exposition.
        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        // Description de l'exposition.
        [Required]
        public required string Description { get; set; }

        // Lieu de l'exposition.
        [Required]
        [StringLength(200)]
        public required string Location { get; set; }

        // Date de début de l'exposition.
        [Required]
        public DateTime StartDate { get; set; }

        // Date de fin de l'exposition.
        [Required]
        public DateTime EndDate { get; set; }

        // Chemin de l'image de l'exposition (optionnel).
        public string? ImagePath { get; set; }

        // Indique si l'exposition est active.
        public bool IsActive { get; set; } = true;
        // Date de création de l'exposition.
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // Date de dernière modification de l'exposition (optionnel).
        public DateTime? UpdatedAt { get; set; }

        // Liste des relations exposition-œuvre.
        public required ICollection<ExhibitionArtwork> ExhibitionArtworks { get; set; }
    }
} 