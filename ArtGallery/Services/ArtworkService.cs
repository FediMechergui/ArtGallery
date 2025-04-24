using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly ApplicationDbContext _context;

        public ArtworkService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Artwork>> GetArtworksAsync(int? category, bool? forSale)
        {
            var artworksQuery = _context.Artworks
                .Include(a => a.Categories)
                .Include(a => a.Images)
                .OrderByDescending(a => a.CreatedAt)
                .AsQueryable();

            if (category.HasValue)
            {
                artworksQuery = artworksQuery.Where(a => a.Categories.Any(c => c.Id == category));
            }
            if (forSale.HasValue)
            {
                artworksQuery = artworksQuery.Where(a => a.IsForSale == forSale);
            }

            return await artworksQuery.ToListAsync();
        }

        public async Task<Artwork?> GetArtworkDetailsAsync(int? id)
        {
            if (id == null) return null;
            return await _context.Artworks
                .Include(a => a.Categories)
                .Include(a => a.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task CreateArtworkAsync(Artwork artwork, int[] selectedCategories, IFormFile? imageFile)
        {
            artwork.Categories = await _context.Categories
                .Where(c => selectedCategories.Contains(c.Id))
                .ToListAsync();
            _context.Add(artwork);
            await _context.SaveChangesAsync();

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/artworks");
                Directory.CreateDirectory(uploads);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                var artworkImage = new ArtworkImage
                {
                    ArtworkId = artwork.Id,
                    Artwork = artwork,
                    ImagePath = "/images/artworks/" + fileName,
                    IsPrimary = true,
                    DisplayOrder = 0
                };
                _context.Add(artworkImage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateArtworkAsync(int id, Artwork artwork, int[] selectedCategories)
        {
            var existingArtwork = await _context.Artworks
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (existingArtwork == null) throw new Exception("Artwork not found");

            existingArtwork.Title = artwork.Title;
            existingArtwork.Description = artwork.Description;
            existingArtwork.TechniqueUsed = artwork.TechniqueUsed;
            existingArtwork.Size = artwork.Size;
            existingArtwork.Price = artwork.Price;
            existingArtwork.CreationDate = artwork.CreationDate;
            existingArtwork.IsAvailable = artwork.IsAvailable;
            existingArtwork.IsForSale = artwork.IsForSale;
            existingArtwork.IsFeatured = artwork.IsFeatured;
            existingArtwork.UpdatedAt = DateTime.Now;

            existingArtwork.Categories.Clear();
            var selectedCategoryEntities = await _context.Categories
                .Where(c => selectedCategories.Contains(c.Id))
                .ToListAsync();
            foreach (var category in selectedCategoryEntities)
            {
                existingArtwork.Categories.Add(category);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteArtworkAsync(int id)
        {
            var artwork = await _context.Artworks.FindAsync(id);
            if (artwork == null) return false;
            _context.Artworks.Remove(artwork);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool ArtworkExists(int id)
        {
            return _context.Artworks.Any(e => e.Id == id);
        }
    }
}
