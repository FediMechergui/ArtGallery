using System.ComponentModel.DataAnnotations;

namespace ArtGallery.Models
{
    public class GoldenBookEntry
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; }
    }
} 