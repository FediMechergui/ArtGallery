using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    public class ExternalLink
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [Required]
        [Url]
        [StringLength(500)]
        public required string Url { get; set; }

        [Required]
        [StringLength(500)]
        public required string Description { get; set; }

        [Required]
        [StringLength(50)]
        public required string Type { get; set; }

        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
} 