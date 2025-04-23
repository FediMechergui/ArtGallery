using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;

namespace ArtGallery.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;

                // Constructeur : injection du contexte de base de données
        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

                /// <summary>
        /// Affiche le panier de l'utilisateur connecté.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _context.ShoppingCarts
                .Include(sc => sc.Artwork)
                .Where(sc => sc.UserId == userId)
                .ToListAsync();

            return View(cartItems);
        }

        [HttpPost]
                /// <summary>
        /// Ajoute une œuvre au panier de l'utilisateur.
        /// </summary>
        /// <param name="artworkId">Identifiant de l'œuvre à ajouter</param>
        /// <param name="quantity">Quantité à ajouter (par défaut 1)</param>
        public async Task<IActionResult> AddToCart(int artworkId, int quantity = 1)
        {
            var artwork = await _context.Artworks.FindAsync(artworkId);
            if (artwork == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var existingCartItem = await _context.ShoppingCarts
                .FirstOrDefaultAsync(sc => sc.UserId == userId && sc.ArtworkId == artworkId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new ShoppingCart
                {
                    UserId = userId!,
                    User = user!,
                    ArtworkId = artworkId,
                    Artwork = artwork!,
                    Quantity = quantity,
                    UnitPrice = artwork.Price
                };
                _context.ShoppingCarts.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
                /// <summary>
        /// Met à jour la quantité d'une œuvre dans le panier.
        /// </summary>
        /// <param name="id">Identifiant de l'entrée du panier</param>
        /// <param name="quantity">Nouvelle quantité</param>
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            var cartItem = await _context.ShoppingCarts.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
                /// <summary>
        /// Supprime une œuvre du panier de l'utilisateur.
        /// </summary>
        /// <param name="id">Identifiant de l'entrée du panier à supprimer</param>
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartItem = await _context.ShoppingCarts.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            _context.ShoppingCarts.Remove(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
                /// <summary>
        /// Valide le panier et crée une commande à partir des œuvres sélectionnées.
        /// </summary>
        /// <param name="artworkId">Identifiant d'une œuvre pour achat rapide (optionnel)</param>
        public async Task<IActionResult> Checkout(int? artworkId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            List<ShoppingCart> cartItems;
            if (artworkId.HasValue)
            {
                var artwork = await _context.Artworks.FindAsync(artworkId.Value);
                if (artwork == null || !artwork.IsAvailable || !artwork.IsForSale)
                {
                    return NotFound();
                }
                cartItems = new List<ShoppingCart>
                {
                    new ShoppingCart
                    {
                        UserId = userId!,
                        User = user!,
                        ArtworkId = artwork.Id,
                        Artwork = artwork!,
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
                return RedirectToAction(nameof(Index));
            }

            var order = new Order
            {
                UserId = userId!,
                User = user!,
                OrderDate = DateTime.Now,
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

            return RedirectToAction("Details", "Order", new { id = order.Id });
        }
    }
}