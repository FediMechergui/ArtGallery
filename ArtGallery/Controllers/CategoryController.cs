using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Authorization;
using ArtGallery.Services;

namespace ArtGallery.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        // Constructeur : injection du service
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Affiche la liste des catégories d'œuvres d'art.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return View(categories);
        }

        /// <summary>
        /// Affiche les détails d'une catégorie spécifique.
        /// </summary>
        /// <param name="id">Identifiant de la catégorie</param>
        public async Task<IActionResult> Details(int? id)
        {
            var category = await _categoryService.GetCategoryDetailsAsync(id);
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
                await _categoryService.CreateCategoryAsync(category);
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
            var category = await _categoryService.GetCategoryDetailsAsync(id);
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
                    await _categoryService.UpdateCategoryAsync(id, category);
                }
                catch (Exception)
                {
                    if (!_categoryService.CategoryExists(category.Id))
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
            var category = await _categoryService.GetCategoryDetailsAsync(id);
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
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
            {
                return NotFound("La catégorie n'a pas été trouvée.");
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Vérifie si une catégorie existe dans la base de données.
        /// </summary>
        /// <param name="id">Identifiant de la catégorie</param>
        /// <returns>Vrai si la catégorie existe, faux sinon</returns>
        private bool CategoryExists(int id)
        {
            return _categoryService.CategoryExists(id);
        }
    }
}