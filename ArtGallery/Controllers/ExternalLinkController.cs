using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ArtGallery.Services;

namespace ArtGallery.Controllers
{
    public class ExternalLinkController : Controller
    {
        private readonly IExternalLinkService _externalLinkService;

        // Constructeur : injection du service
        public ExternalLinkController(IExternalLinkService externalLinkService)
        {
            _externalLinkService = externalLinkService;
        }

        // GET: ExternalLink
        /// <summary>
        /// Affiche la liste des liens externes.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var links = await _externalLinkService.GetAllAsync();
            return View(links);
        }

        // GET: ExternalLink/Create
        [Authorize(Roles = "Admin")]
                /// <summary>
        /// Affiche le formulaire de création d'un nouveau lien externe (admin).
        /// </summary>
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExternalLink/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        /// <summary>
        /// Traite la soumission du formulaire de création d'un lien externe (admin).
        /// </summary>
        /// <param name="externalLink">Objet ExternalLink à créer</param>
        public async Task<IActionResult> Create([Bind("Id,Title,Url,Description,Type,IsActive,SortOrder")] ExternalLink externalLink)
        {
            if (ModelState.IsValid)
            {
                await _externalLinkService.CreateAsync(externalLink);
                return RedirectToAction(nameof(Index));
            }
            return View(externalLink);
        }

        // GET: ExternalLink/Edit/5
        [Authorize(Roles = "Admin")]
                /// <summary>
        /// Affiche le formulaire d'édition d'un lien externe existant (admin).
        /// </summary>
        /// <param name="id">Identifiant du lien externe à éditer</param>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var externalLink = await _externalLinkService.GetByIdAsync(id.Value);
            if (externalLink == null)
            {
                return NotFound();
            }
            return View(externalLink);
        }

        // POST: ExternalLink/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        /// <summary>
        /// Traite la soumission du formulaire d'édition d'un lien externe (admin).
        /// </summary>
        /// <param name="id">Identifiant du lien externe</param>
        /// <param name="externalLink">Objet ExternalLink modifié</param>
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Url,Description,Type,IsActive,SortOrder")] ExternalLink externalLink)
        {
            if (id != externalLink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _externalLinkService.UpdateAsync(externalLink);
                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _externalLinkService.ExistsAsync(externalLink.Id);
                    if (!exists)
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
            return View(externalLink);
        }

        // GET: ExternalLink/Delete/5
        [Authorize(Roles = "Admin")]
                /// <summary>
        /// Affiche la page de confirmation de suppression d'un lien externe (admin).
        /// </summary>
        /// <param name="id">Identifiant du lien externe à supprimer</param>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var externalLink = await _externalLinkService.GetByIdAsync(id.Value);
            if (externalLink == null)
            {
                return NotFound();
            }

            return View(externalLink);
        }

        // POST: ExternalLink/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        /// <summary>
        /// Traite la suppression définitive d'un lien externe (admin).
        /// </summary>
        /// <param name="id">Identifiant du lien externe à supprimer</param>
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _externalLinkService.DeleteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

                /// <summary>
        /// Vérifie si un lien externe existe dans la base de données.
        /// </summary>
        /// <param name="id">Identifiant du lien externe</param>
        /// <returns>Vrai si le lien existe, faux sinon</returns>
        private async Task<bool> ExternalLinkExists(int id)
        {
            return await _externalLinkService.ExistsAsync(id);
        }
    }
} 