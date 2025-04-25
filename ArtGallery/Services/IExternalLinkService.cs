using ArtGallery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtGallery.Services
{
    public interface IExternalLinkService
    {
        Task<List<ExternalLink>> GetAllAsync();
        Task<ExternalLink?> GetByIdAsync(int id);
        Task CreateAsync(ExternalLink externalLink);
        Task UpdateAsync(ExternalLink externalLink);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
