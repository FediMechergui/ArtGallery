using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtGallery.Services
{
    public class ExternalLinkService : IExternalLinkService
    {
        private readonly ApplicationDbContext _context;

        public ExternalLinkService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExternalLink>> GetAllAsync()
        {
            return await _context.ExternalLinks.ToListAsync();
        }

        public async Task<ExternalLink?> GetByIdAsync(int id)
        {
            return await _context.ExternalLinks.FindAsync(id);
        }

        public async Task CreateAsync(ExternalLink externalLink)
        {
            externalLink.CreatedAt = System.DateTime.Now;
            externalLink.UpdatedAt = null;
            _context.ExternalLinks.Add(externalLink);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ExternalLink externalLink)
        {
            var existing = await _context.ExternalLinks.AsNoTracking().FirstOrDefaultAsync(e => e.Id == externalLink.Id);
            if (existing == null)
                throw new KeyNotFoundException();
            externalLink.CreatedAt = existing.CreatedAt;
            externalLink.UpdatedAt = System.DateTime.Now;
            _context.ExternalLinks.Update(externalLink);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var externalLink = await _context.ExternalLinks.FindAsync(id);
            if (externalLink == null)
                throw new KeyNotFoundException();
            _context.ExternalLinks.Remove(externalLink);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.ExternalLinks.AnyAsync(e => e.Id == id);
        }
    }
}
