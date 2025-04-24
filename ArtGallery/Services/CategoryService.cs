using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category?> GetCategoryDetailsAsync(int? id)
        {
            if (id == null) return null;
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCategoryAsync(int id, Category category)
        {
            if (id != category.Id) throw new Exception("Category ID mismatch");
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
