using System.Collections.Generic;

namespace ArtGallery.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            FeaturedArtworks = new List<Artwork>();
            LatestExhibitions = new List<Exhibition>();
        }

        public required ICollection<Artwork> FeaturedArtworks { get; set; }
        public required ICollection<Exhibition> LatestExhibitions { get; set; }
    }
} 