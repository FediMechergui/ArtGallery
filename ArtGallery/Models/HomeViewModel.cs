using System.Collections.Generic;

namespace ArtGallery.Models
{
    // Modèle de vue pour la page d'accueil, contenant les œuvres et expositions à afficher.
    public class HomeViewModel
    {
        // Initialise une nouvelle instance de HomeViewModel et les collections associées.
        public HomeViewModel()
        {
            FeaturedArtworks = new List<Artwork>();
            LatestExhibitions = new List<Exhibition>();
            ForSaleArtworks = new List<Artwork>();
            AvailableArtworks = new List<Artwork>();
        }

        // Liste des œuvres en vedette.
        public required ICollection<Artwork> FeaturedArtworks { get; set; }
        // Liste des œuvres en vente.
        public required ICollection<Artwork> ForSaleArtworks { get; set; }
        // Liste des œuvres disponibles.
        public required ICollection<Artwork> AvailableArtworks { get; set; }
        // Liste des dernières expositions.
        public required ICollection<Exhibition> LatestExhibitions { get; set; }
    }
}