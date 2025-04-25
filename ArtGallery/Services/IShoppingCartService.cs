using ArtGallery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtGallery.Services
{
    public interface IShoppingCartService
    {
        Task<List<ShoppingCart>> GetCartItemsAsync(string userId);
        Task<bool> AddToCartAsync(string userId, int artworkId, int quantity = 1);
        Task<bool> UpdateQuantityAsync(int cartItemId, int quantity);
        Task<bool> RemoveFromCartAsync(int cartItemId);
        Task<int?> CheckoutAsync(string userId, int? artworkId = null);
    }
}
