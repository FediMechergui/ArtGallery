using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtGallery.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShoppingCart>> GetCartItemsAsync(string userId)
        {
            return await _context.ShoppingCarts
                .Include(sc => sc.Artwork)
                .Where(sc => sc.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> AddToCartAsync(string userId, int artworkId, int quantity = 1)
        {
            var artwork = await _context.Artworks.FindAsync(artworkId);
            if (artwork == null) return false;
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            var existingCartItem = await _context.ShoppingCarts.FirstOrDefaultAsync(sc => sc.UserId == userId && sc.ArtworkId == artworkId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new ShoppingCart
                {
                    UserId = userId,
                    User = user,
                    ArtworkId = artworkId,
                    Artwork = artwork,
                    Quantity = quantity,
                    UnitPrice = artwork.Price
                };
                _context.ShoppingCarts.Add(cartItem);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityAsync(int cartItemId, int quantity)
        {
            var cartItem = await _context.ShoppingCarts.FindAsync(cartItemId);
            if (cartItem == null) return false;
            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await _context.ShoppingCarts.FindAsync(cartItemId);
            if (cartItem == null) return false;
            _context.ShoppingCarts.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int?> CheckoutAsync(string userId, int? artworkId = null)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;
            List<ShoppingCart> cartItems;
            if (artworkId.HasValue)
            {
                var artwork = await _context.Artworks.FindAsync(artworkId.Value);
                if (artwork == null || !artwork.IsAvailable || !artwork.IsForSale)
                {
                    return null;
                }
                cartItems = new List<ShoppingCart>
                {
                    new ShoppingCart
                    {
                        UserId = userId,
                        User = user,
                        ArtworkId = artwork.Id,
                        Artwork = artwork,
                        Quantity = 1,
                        UnitPrice = artwork.Price
                    }
                };
            }
            else
            {
                cartItems = await _context.ShoppingCarts
                    .Include(sc => sc.Artwork)
                    .Where(sc => sc.UserId == userId)
                    .ToListAsync();
            }
            if (!cartItems.Any())
            {
                return null;
            }
            var order = new Order
            {
                UserId = userId,
                User = user,
                OrderDate = System.DateTime.Now,
                Status = OrderStatus.Pending,
                TotalAmount = cartItems.Sum(item => item.Quantity * item.UnitPrice),
                ShippingAddress = "To be provided",
                ShippingCity = "To be provided",
                ShippingState = "To be provided",
                ShippingPostalCode = "To be provided",
                ShippingCountry = "To be provided",
                OrderDetails = new List<OrderDetail>()
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Save to get the order ID
            var orderDetails = cartItems.Select(item => new OrderDetail
            {
                OrderId = order.Id,
                Order = order,
                ArtworkId = item.ArtworkId,
                Artwork = item.Artwork,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Subtotal = item.Quantity * item.UnitPrice
            }).ToList();
            order.OrderDetails = orderDetails;
            await _context.SaveChangesAsync();
            if (!artworkId.HasValue)
            {
                _context.ShoppingCarts.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
            return order.Id;
        }
    }
}
