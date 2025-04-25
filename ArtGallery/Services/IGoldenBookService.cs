using ArtGallery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtGallery.Services
{
    public interface IGoldenBookService
    {
        Task<List<GoldenBookEntry>> GetAllAsync();
        Task<GoldenBookEntry?> GetByIdAsync(int id);
        Task CreateAsync(GoldenBookEntry entry);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
