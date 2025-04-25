using ArtGallery.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtGallery.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersForUserAsync(string userId);
        Task<Order?> GetOrderDetailsAsync(int orderId, string userId, bool isAdmin);
        Task<List<Order>> GetAllOrdersAsync();
        Task<bool> UpdateStatusAsync(int orderId, OrderStatus status, string? adminName);
        Task<bool> ApproveAsync(int orderId, string? adminName);
        Task<bool> DeclineAsync(int orderId);
    }
}
