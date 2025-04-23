using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    // Représente une catégorie d'œuvres d'art.
    public class Category
    {
        // Initialise une nouvelle instance de la classe Category et la collection d'œuvres associées.
        public Category()
        {
            Artworks = new List<Artwork>();
        }

        // Identifiant unique de la catégorie.
        public int Id { get; set; }

        // Nom de la catégorie.
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        // Description de la catégorie.
        [StringLength(200)]
        public string? Description { get; set; }

        // Indique si la catégorie est active.
        public bool IsActive { get; set; } = true;

        // Liste des œuvres associées à cette catégorie.
        public required ICollection<Artwork> Artworks { get; set; }
    }
}