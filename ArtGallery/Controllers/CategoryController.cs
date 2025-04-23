using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Authorization;

namespace ArtGallery.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructeur : injection du contexte de base de données
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Affiche la liste des catégories d'œuvres d'art.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        /// <summary>
        /// Affiche les détails d'une catégorie spécifique.
        /// </summary>
        /// <param name="id">Identifiant de la catégorie</param>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound("La catégorie n'a pas été trouvée.");
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound("La catégorie n'a pas été trouvée.");
            }

            return View(category);
        }

        /// <summary>
        /// Affiche le formulaire de création d'une nouvelle catégorie.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Traite la soumission du formulaire de création d'une catégorie.
        /// </summary>
        /// <param name="category">Objet Category à créer</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,IsActive")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        /// <summary>
        /// Affiche le formulaire d'édition d'une catégorie existante.
        /// </summary>
        /// <param name="id">Identifiant de la catégorie à éditer</param>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound("La catégorie n'a pas été trouvée.");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound("La catégorie n'a pas été trouvée.");
            }
            return View(category);
        }

        /// <summary>
        /// Traite la soumission du formulaire d'édition d'une catégorie.
        /// </summary>
        /// <param name="id">Identifiant de la catégorie</param>
        /// <param name="category">Objet Category modifié</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsActive")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound("La catégorie n'a pas été trouvée.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound("La catégorie n'a pas été trouvée.");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        /// <summary>
        /// Affiche la page de confirmation de suppression d'une catégorie.
        /// </summary>
        /// <param name="id">Identifiant de la catégorie à supprimer</param>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("La catégorie n'a pas été trouvée.");
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound("La catégorie n'a pas été trouvée.");
            }

            return View(category);
        }

        /// <summary>
        /// Traite la suppression définitive d'une catégorie.
        /// </summary>
        /// <param name="id">Identifiant de la catégorie à supprimer</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Vérifie si une catégorie existe dans la base de données.
        /// </summary>
        /// <param name="id">Identifiant de la catégorie</param>
        /// <returns>Vrai si la catégorie existe, faux sinon</returns>
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}