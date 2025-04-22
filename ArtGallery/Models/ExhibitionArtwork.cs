using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    public class ExhibitionArtwork
    {
        [Required]
        public int ExhibitionId { get; set; }
        public required Exhibition Exhibition { get; set; }

        [Required]
        public int ArtworkId { get; set; }
        public required Artwork Artwork { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsFeatured { get; set; }
    }
} 