using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using ArtGallery.Services;

namespace ArtGallery.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        // Constructeur : injection du service
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

                /// <summary>
        /// Affiche le panier de l'utilisateur connecté.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = await _shoppingCartService.GetCartItemsAsync(userId);
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _shoppingCartService.AddToCartAsync(userId, artworkId, quantity);
            if (!result)
            {
                return NotFound();
            }
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
            var result = await _shoppingCartService.UpdateQuantityAsync(id, quantity);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
                /// <summary>
        /// Supprime une œuvre du panier de l'utilisateur.
        /// </summary>
        /// <param name="id">Identifiant de l'entrée du panier à supprimer</param>
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var result = await _shoppingCartService.RemoveFromCartAsync(id);
            if (!result)
            {
                return NotFound();
            }
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
            var orderId = await _shoppingCartService.CheckoutAsync(userId, artworkId);
            if (orderId == null)
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Order", new { id = orderId.Value });
        }
    }
}