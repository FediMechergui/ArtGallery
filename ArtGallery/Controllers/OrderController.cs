using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

namespace ArtGallery.Controllers
{
    // Contrôleur pour la gestion des commandes (Commandes des utilisateurs et gestion admin)
    [Authorize]
    public class OrderController : Controller
    {
        // Contexte de base de données pour accéder aux entités
        private readonly ApplicationDbContext _context;

        // Constructeur : injection du contexte de base de données
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Affiche la liste des commandes de l'utilisateur connecté
        // Accessible à tout utilisateur authentifié
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _context.Orders
                .Include(o => o.OrderDetails) // Inclut les détails de chaque commande
                .ThenInclude(od => od.Artwork) // Inclut les œuvres associées à chaque détail
                .Where(o => o.UserId == userId) // Filtre les commandes de l'utilisateur courant
                .OrderByDescending(o => o.OrderDate) // Trie par date décroissante
                .ToListAsync();

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

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Artwork)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Seul l'admin ou le propriétaire peut voir la commande
            if (!User.IsInRole("Admin") && order.UserId != userId)
            {
                return Forbid();
            }

            return View(order);
        }

        // Affiche la liste de toutes les commandes (Vue admin uniquement)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Manage()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Artwork)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // Met à jour le statut d'une commande (Admin uniquement)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            // Si la commande passe en traitement, on enregistre l'approbation
            if (status == OrderStatus.Processing)
            {
                order.ApprovedAt = DateTime.Now;
                order.ApprovedBy = User.Identity?.Name;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }

        // Approuve une commande (Admin uniquement)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = OrderStatus.Approved;
            order.ApprovedAt = DateTime.Now;
            order.ApprovedBy = User.Identity?.Name;
            await _context.SaveChangesAsync();
            // Optionnel : notifier l'utilisateur
            return RedirectToAction(nameof(Manage));
        }

        // Refuse une commande (Admin uniquement)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Decline(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            order.Status = OrderStatus.Declined;
            await _context.SaveChangesAsync();
            // Optionnel : notifier l'utilisateur
            return RedirectToAction(nameof(Manage));
        }
    }
}