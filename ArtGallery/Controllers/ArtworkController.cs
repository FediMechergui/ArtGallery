using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Authorization;
using ArtGallery.Services;

namespace ArtGallery.Controllers
{
    public class ArtworkController : Controller
    {
        private readonly IArtworkService _artworkService;

        // Constructeur : injection du service
        public ArtworkController(IArtworkService artworkService)
        {
            _artworkService = artworkService;
        }

                /// <summary>
        /// Affiche la liste des œuvres d'art, avec possibilité de filtrer par catégorie ou disponibilité à la vente.
        /// </summary>
        /// <param name="category">Identifiant de la catégorie (optionnel)</param>
        /// <param name="forSale">Filtre pour les œuvres en vente (optionnel)</param>
        public async Task<IActionResult> Index(int? category, bool? forSale)
        {
            var artworks = await _artworkService.GetArtworksAsync(category, forSale);
            var categories = await _artworkService.GetCategoriesAsync();
            ViewBag.Categories = categories;
            return View(artworks);
        }

                /// <summary>
        /// Affiche les détails d'une œuvre d'art spécifique.
        /// </summary>
        /// <param name="id">Identifiant de l'œuvre</param>
        public async Task<IActionResult> Details(int? id)
        {
            var artwork = await _artworkService.GetArtworkDetailsAsync(id);
            if (artwork == null)
            {
                return NotFound();
            }
            return View(artwork);
        }

                /// <summary>
        /// Affiche le formulaire de création d'une nouvelle œuvre (réservé à l'admin).
        /// </summary>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _artworkService.GetCategoriesAsync();
            return View();
        }

                /// <summary>
        /// Traite la soumission du formulaire de création d'une œuvre (admin).
        /// Associe les catégories sélectionnées et gère l'upload d'image principale.
        /// </summary>
        /// <param name="artwork">Objet Artwork à créer</param>
        /// <param name="selectedCategories">Catégories sélectionnées</param>
        /// <param name="imageFile">Fichier image téléchargé</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Title,Description,TechniqueUsed,Size,Price,CreationDate,IsAvailable,IsForSale,IsFeatured")] Artwork artwork, int[] selectedCategories, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                await _artworkService.CreateArtworkAsync(artwork, selectedCategories, imageFile);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _artworkService.GetCategoriesAsync();
            return View(artwork);
        }

                /// <summary>
        /// Affiche le formulaire d'édition d'une œuvre existante (admin).
        /// </summary>
        /// <param name="id">Identifiant de l'œuvre à éditer</param>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            var artwork = await _artworkService.GetArtworkDetailsAsync(id);
            if (artwork == null)
            {
                return NotFound();
            }
            ViewBag.Categories = await _artworkService.GetCategoriesAsync();
            return View(artwork);
        }

                /// <summary>
        /// Traite la soumission du formulaire d'édition d'une œuvre (admin).
        /// Met à jour les champs et les catégories associées.
        /// </summary>
        /// <param name="id">Identifiant de l'œuvre</param>
        /// <param name="artwork">Objet Artwork modifié</param>
        /// <param name="selectedCategories">Catégories sélectionnées</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,TechniqueUsed,Size,Price,CreationDate,IsAvailable,IsForSale,IsFeatured")] Artwork artwork, int[] selectedCategories)
        {
            if (id != artwork.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _artworkService.UpdateArtworkAsync(id, artwork, selectedCategories);
                }
                catch (Exception)
                {
                    if (!_artworkService.ArtworkExists(artwork.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _artworkService.GetCategoriesAsync();
            return View(artwork);
        }

                /// <summary>
        /// Affiche la page de confirmation de suppression d'une œuvre (admin).
        /// </summary>
        /// <param name="id">Identifiant de l'œuvre à supprimer</param>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            var artwork = await _artworkService.GetArtworkDetailsAsync(id);
            if (artwork == null)
            {
                return NotFound();
            }
            return View(artwork);
        }

                /// <summary>
        /// Traite la suppression définitive d'une œuvre (admin).
        /// </summary>
        /// <param name="id">Identifiant de l'œuvre à supprimer</param>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _artworkService.DeleteArtworkAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

                /// <summary>
        /// Vérifie si une œuvre existe dans la base de données.
        /// </summary>
        /// <param name="id">Identifiant de l'œuvre</param>
        /// <returns>Vrai si l'œuvre existe, faux sinon</returns>
        private bool ArtworkExists(int id)
        {
            return _artworkService.ArtworkExists(id);
        }
    }
} 