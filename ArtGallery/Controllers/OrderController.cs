using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using ArtGallery.Services;

namespace ArtGallery.Controllers
{
    // Contrôleur pour la gestion des commandes (Commandes des utilisateurs et gestion admin)
    [Authorize]
    public class OrderController : Controller
    {
        // Contexte de base de données pour accéder aux entités
        private readonly IOrderService _orderService;

        // Constructeur : injection du service
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Affiche la liste des commandes de l'utilisateur connecté
        // Accessible à tout utilisateur authentifié
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetOrdersForUserAsync(userId);
            return View(orders);
        }

        // Affiche les détails d'une commande spécifique
        // L'utilisateur ne peut voir que ses propres commandes (sauf admin)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");
            var order = await _orderService.GetOrderDetailsAsync(id.Value, userId, isAdmin);
            if (order == null)
            {
                if (id == null)
                    return NotFound();
                if (!isAdmin)
                    return Forbid();
                return NotFound();
            }
            return View(order);
        }

        // Affiche la liste de toutes les commandes (Vue admin uniquement)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Manage()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        // Met à jour le statut d'une commande (Admin uniquement)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var adminName = User.Identity?.Name;
            var updated = await _orderService.UpdateStatusAsync(id, status, adminName);
            if (!updated)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Manage));
        }

        // Approuve une commande (Admin uniquement)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var adminName = User.Identity?.Name;
            var approved = await _orderService.ApproveAsync(id, adminName);
            if (!approved)
            {
                return NotFound();
            }
            // Optionnel : notifier l'utilisateur
            return RedirectToAction(nameof(Manage));
        }

        // Refuse une commande (Admin uniquement)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Decline(int id)
        {
            var declined = await _orderService.DeclineAsync(id);
            if (!declined)
            {
                return NotFound();
            }
            // Optionnel : notifier l'utilisateur
            return RedirectToAction(nameof(Manage));
        }
    }
}