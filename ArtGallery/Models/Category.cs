using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    public class Category
    {
        public Category()
        {
            Artworks = new List<Artwork>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public required ICollection<Artwork> Artworks { get; set; }
    }
} 