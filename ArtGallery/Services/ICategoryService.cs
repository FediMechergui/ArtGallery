using ArtGallery.Models;
namespace ArtGallery.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<Category?> GetCategoryDetailsAsync(int? id);
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(int id, Category category);
        Task<bool> DeleteCategoryAsync(int id);
        bool CategoryExists(int id);
    }
}
