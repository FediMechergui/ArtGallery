using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Services
{
    public class ExhibitionService : IExhibitionService
    {
        private readonly ApplicationDbContext _context;
        public ExhibitionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Exhibition>> GetExhibitionsAsync()
        {
            return await _context.Exhibitions
                .Include(e => e.ExhibitionArtworks)
                .ThenInclude(ea => ea.Artwork)
                .ToListAsync();
        }
        public async Task<Exhibition?> GetExhibitionDetailsAsync(int? id)
        {
            if (id == null) return null;
            return await _context.Exhibitions
                .Include(e => e.ExhibitionArtworks)
                .ThenInclude(ea => ea.Artwork)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task CreateExhibitionAsync(Exhibition exhibition, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/exhibitions");
                Directory.CreateDirectory(uploadsDir);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                exhibition.ImagePath = "/images/exhibitions/" + fileName;
            }
            _context.Exhibitions.Add(exhibition);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateExhibitionAsync(int id, Exhibition exhibition, IFormFile? imageFile)
        {
            var existingExhibition = await _context.Exhibitions.FindAsync(id);
            if (existingExhibition == null) throw new Exception("Exhibition not found");
            existingExhibition.Title = exhibition.Title;
            existingExhibition.Description = exhibition.Description;
            existingExhibition.StartDate = exhibition.StartDate;
            existingExhibition.EndDate = exhibition.EndDate;
            existingExhibition.Location = exhibition.Location;
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/exhibitions");
                Directory.CreateDirectory(uploadsDir);
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                existingExhibition.ImagePath = "/images/exhibitions/" + fileName;
            }
            _context.Exhibitions.Update(existingExhibition);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteExhibitionAsync(int id)
        {
            var exhibition = await _context.Exhibitions.FindAsync(id);
            if (exhibition == null) return false;
            _context.Exhibitions.Remove(exhibition);
            await _context.SaveChangesAsync();
            return true;
        }
        public bool ExhibitionExists(int id)
        {
            return _context.Exhibitions.Any(e => e.Id == id);
        }
    }
}
