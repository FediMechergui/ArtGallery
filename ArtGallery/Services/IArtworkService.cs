using ArtGallery.Models;
using Microsoft.AspNetCore.Http;

namespace ArtGallery.Services
{
    public interface IArtworkService
    {
        Task<List<Artwork>> GetArtworksAsync(int? category, bool? forSale);
        Task<Artwork?> GetArtworkDetailsAsync(int? id);
        Task<List<Category>> GetCategoriesAsync();
        Task CreateArtworkAsync(Artwork artwork, int[] selectedCategories, IFormFile? imageFile);
        Task UpdateArtworkAsync(int id, Artwork artwork, int[] selectedCategories);
        Task<bool> DeleteArtworkAsync(int id);
        bool ArtworkExists(int id);
    }
}
