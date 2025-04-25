using ArtGallery.Models;
using System.Threading.Tasks;

namespace ArtGallery.Services
{
    public interface IHomeService
    {
        Task<HomeViewModel> GetHomeViewModelAsync();
    }
}
