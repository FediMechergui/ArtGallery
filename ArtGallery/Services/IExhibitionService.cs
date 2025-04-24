using ArtGallery.Models;
using Microsoft.AspNetCore.Http;
namespace ArtGallery.Services
{
    public interface IExhibitionService
    {
        Task<List<Exhibition>> GetExhibitionsAsync();
        Task<Exhibition?> GetExhibitionDetailsAsync(int? id);
        Task CreateExhibitionAsync(Exhibition exhibition, IFormFile? imageFile);
        Task UpdateExhibitionAsync(int id, Exhibition exhibition, IFormFile? imageFile);
        Task<bool> DeleteExhibitionAsync(int id);
        bool ExhibitionExists(int id);
    }
}
