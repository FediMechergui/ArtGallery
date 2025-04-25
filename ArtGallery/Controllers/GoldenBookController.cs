using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ArtGallery.Services;

namespace ArtGallery.Controllers
{
    public class GoldenBookController : Controller
    {
        private readonly IGoldenBookService _goldenBookService;

        // Constructeur : injection du service
        public GoldenBookController(IGoldenBookService goldenBookService)
        {
            _goldenBookService = goldenBookService;
        }

        // GET: GoldenBook
        /// <summary>
        /// Affiche la liste des messages du livre d'or, triés par date de création décroissante.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var entries = await _goldenBookService.GetAllAsync();
            return View(entries);
        }

        // GET: GoldenBook/Create
        /// <summary>
        /// Affiche le formulaire pour ajouter un nouveau message au livre d'or.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        // POST: GoldenBook/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Traite la soumission du formulaire pour ajouter un message au livre d'or.
        /// </summary>
        /// <param name="goldenBookEntry">Entrée à ajouter</param>
        public async Task<IActionResult> Create([Bind("Name,Email,Message")] GoldenBookEntry goldenBookEntry)
        {
            if (ModelState.IsValid)
            {
                await _goldenBookService.CreateAsync(goldenBookEntry);
                return RedirectToAction(nameof(Index));
            }
            return View(goldenBookEntry);
        }

        // GET: GoldenBook/Delete/5
        [Authorize(Roles = "Admin")]
        /// <summary>
        /// Affiche la page de confirmation de suppression d'un message du livre d'or (admin).
        /// </summary>
        /// <param name="id">Identifiant du message à supprimer</param>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound("La page demandée n'a pas été trouvée.");
            }

            var goldenBookEntry = await _goldenBookService.GetByIdAsync(id.Value);
            if (goldenBookEntry == null)
            {
                return NotFound("La page demandée n'a pas été trouvée.");
            }

            return View(goldenBookEntry);
        }

        // POST: GoldenBook/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        /// <summary>
        /// Traite la suppression définitive d'un message du livre d'or (admin).
        /// </summary>
        /// <param name="id">Identifiant du message à supprimer</param>
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _goldenBookService.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                // Optionnel : afficher un message d'erreur ou rediriger
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Vérifie si un message du livre d'or existe dans la base de données.
        /// </summary>
        /// <param name="id">Identifiant du message</param>
        /// <returns>Vrai si le message existe, faux sinon</returns>
        private async Task<bool> GoldenBookEntryExists(int id)
        {
            return await _goldenBookService.ExistsAsync(id);
        }
    }
}