using System;
using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    public class Exhibition
    {
        public Exhibition()
        {
            ExhibitionArtworks = new List<ExhibitionArtwork>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        [StringLength(200)]
        public required string Location { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public required string ImagePath { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public required ICollection<ExhibitionArtwork> ExhibitionArtworks { get; set; }
    }
} 