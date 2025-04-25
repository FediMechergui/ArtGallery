using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ArtGallery.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _context;

        public HomeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HomeViewModel> GetHomeViewModelAsync()
        {
            var viewModel = new HomeViewModel
            {
                FeaturedArtworks = await _context.Artworks
                    .Include(a => a.Categories)
                    .Include(a => a.Images)
                    .Where(a => a.IsFeatured && a.IsAvailable)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync(),

                ForSaleArtworks = await _context.Artworks
                    .Include(a => a.Categories)
                    .Include(a => a.Images)
                    .Where(a => a.IsForSale && a.IsAvailable)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync(),

                AvailableArtworks = await _context.Artworks
                    .Include(a => a.Categories)
                    .Include(a => a.Images)
                    .Where(a => a.IsAvailable)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync(),

                LatestExhibitions = await _context.Exhibitions
                    .Include(e => e.ExhibitionArtworks)
                    .ThenInclude(ea => ea.Artwork)
                    .Where(e => e.IsActive && e.EndDate >= System.DateTime.Now)
                    .OrderBy(e => e.StartDate)
                    .Take(3)
                    .ToListAsync()
            };

            return viewModel;
        }
    }
}
