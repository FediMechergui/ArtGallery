using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtGallery.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersForUserAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Artwork)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderDetailsAsync(int orderId, string userId, bool isAdmin)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Artwork)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
                return null;
            if (!isAdmin && order.UserId != userId)
                return null;
            return order;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Artwork)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(int orderId, OrderStatus status, string? adminName)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;
            order.Status = status;
            if (status == OrderStatus.Processing)
            {
                order.ApprovedAt = System.DateTime.Now;
                order.ApprovedBy = adminName;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveAsync(int orderId, string? adminName)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;
            order.Status = OrderStatus.Approved;
            order.ApprovedAt = System.DateTime.Now;
            order.ApprovedBy = adminName;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeclineAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;
            order.Status = OrderStatus.Declined;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
