using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using System.Threading.Tasks;
using ArtGallery.Services;

namespace ArtGallery.Controllers
{
    public class ExhibitionController : Controller
    {
        private readonly IExhibitionService _exhibitionService;

        // Constructeur : injection du service
        public ExhibitionController(IExhibitionService exhibitionService)
        {
            _exhibitionService = exhibitionService;
        }

        // GET: Exhibition
                /// <summary>
        /// Affiche la liste des expositions avec les œuvres associées.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var exhibitions = await _exhibitionService.GetExhibitionsAsync();
            return View(exhibitions);
        }

        // GET: Exhibition/Details/5
                /// <summary>
        /// Affiche les détails d'une exposition spécifique.
        /// </summary>
        /// <param name="id">Identifiant de l'exposition</param>
        public async Task<IActionResult> Details(int? id)
        {
            var exhibition = await _exhibitionService.GetExhibitionDetailsAsync(id);
            if (exhibition == null)
            {
                return NotFound();
            }
            return View(exhibition);
        }

        // GET: Exhibition/Create
                /// <summary>
        /// Affiche le formulaire de création d'une nouvelle exposition.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exhibition/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
                /// <summary>
        /// Traite la soumission du formulaire de création d'une exposition.
        /// Gère l'upload de l'image associée.
        /// </summary>
        /// <param name="exhibition">Objet Exhibition à créer</param>
        /// <param name="imageFile">Fichier image téléchargé</param>
        public async Task<IActionResult> Create([Bind("Id,Title,Description,StartDate,EndDate,Location")] Exhibition exhibition, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError("ImagePath", "An exhibition image is required.");
            }
            if (ModelState.IsValid)
            {
                await _exhibitionService.CreateExhibitionAsync(exhibition, imageFile);
                return RedirectToAction(nameof(Index));
            }
            return View(exhibition);
        }

        // GET: Exhibition/Edit/5
                /// <summary>
        /// Affiche le formulaire d'édition d'une exposition existante.
        /// </summary>
        /// <param name="id">Identifiant de l'exposition à éditer</param>
        public async Task<IActionResult> Edit(int? id)
        {
            var exhibition = await _exhibitionService.GetExhibitionDetailsAsync(id);
            if (exhibition == null)
                return NotFound();
            return View(exhibition);
        }

        // POST: Exhibition/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
                /// <summary>
        /// Traite la soumission du formulaire d'édition d'une exposition.
        /// Met à jour les champs et gère l'image associée.
        /// </summary>
        /// <param name="id">Identifiant de l'exposition</param>
        /// <param name="exhibition">Objet Exhibition modifié</param>
        /// <param name="imageFile">Fichier image téléchargé</param>
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StartDate,EndDate,Location")] Exhibition exhibition, IFormFile imageFile)
        {
            if (id != exhibition.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _exhibitionService.UpdateExhibitionAsync(id, exhibition, imageFile);
                }
                catch (Exception)
                {
                    if (!_exhibitionService.ExhibitionExists(exhibition.Id))
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
            return View(exhibition);
        }

        // GET: Exhibition/Delete/5
                /// <summary>
        /// Affiche la page de confirmation de suppression d'une exposition.
        /// </summary>
        /// <param name="id">Identifiant de l'exposition à supprimer</param>
        public async Task<IActionResult> Delete(int? id)
        {
            var exhibition = await _exhibitionService.GetExhibitionDetailsAsync(id);
            if (exhibition == null)
            {
                return NotFound();
            }
            return View(exhibition);
        }

        // POST: Exhibition/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
                /// <summary>
        /// Traite la suppression définitive d'une exposition.
        /// </summary>
        /// <param name="id">Identifiant de l'exposition à supprimer</param>
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _exhibitionService.DeleteExhibitionAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

                /// <summary>
        /// Vérifie si une exposition existe dans la base de données.
        /// </summary>
        /// <param name="id">Identifiant de l'exposition</param>
        /// <returns>Vrai si l'exposition existe, faux sinon</returns>
        private bool ExhibitionExists(int id)
        {
            return _exhibitionService.ExhibitionExists(id);
        }
    }
} 