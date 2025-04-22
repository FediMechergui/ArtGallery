using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    public class ArtworkImage
    {
        public int Id { get; set; }

        [Required]
        public int ArtworkId { get; set; }
        public required Artwork Artwork { get; set; }

        [Required]
        [StringLength(500)]
        public required string ImagePath { get; set; }

        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 