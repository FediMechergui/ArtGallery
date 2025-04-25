using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtGallery.Services
{
    public class GoldenBookService : IGoldenBookService
    {
        private readonly ApplicationDbContext _context;

        public GoldenBookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GoldenBookEntry>> GetAllAsync()
        {
            return await _context.GoldenBookEntries.OrderByDescending(g => g.CreatedAt).ToListAsync();
        }

        public async Task<GoldenBookEntry?> GetByIdAsync(int id)
        {
            return await _context.GoldenBookEntries.FindAsync(id);
        }

        public async Task CreateAsync(GoldenBookEntry entry)
        {
            entry.CreatedAt = System.DateTime.Now;
            _context.GoldenBookEntries.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entry = await _context.GoldenBookEntries.FindAsync(id);
            if (entry == null)
                throw new KeyNotFoundException();
            _context.GoldenBookEntries.Remove(entry);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.GoldenBookEntries.AnyAsync(e => e.Id == id);
        }
    }
}
