using System.Collections.Generic;

namespace ArtGallery.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            FeaturedArtworks = new List<Artwork>();
            LatestExhibitions = new List<Exhibition>();
            ForSaleArtworks = new List<Artwork>();
            AvailableArtworks = new List<Artwork>();
        }

        public required ICollection<Artwork> FeaturedArtworks { get; set; }
        public required ICollection<Artwork> ForSaleArtworks { get; set; }
        public required ICollection<Artwork> AvailableArtworks { get; set; }
        public required ICollection<Exhibition> LatestExhibitions { get; set; }
    }
}