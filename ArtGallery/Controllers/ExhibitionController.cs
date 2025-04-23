using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using System.Threading.Tasks;

namespace ArtGallery.Controllers
{
    public class ExhibitionController : Controller
    {
        private readonly ApplicationDbContext _context;

                // Constructeur : injection du contexte de base de données
        public ExhibitionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exhibition
                /// <summary>
        /// Affiche la liste des expositions avec les œuvres associées.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Exhibitions
                .Include(e => e.ExhibitionArtworks)
                .ThenInclude(ea => ea.Artwork)
                .ToListAsync());
        }

        // GET: Exhibition/Details/5
                /// <summary>
        /// Affiche les détails d'une exposition spécifique.
        /// </summary>
        /// <param name="id">Identifiant de l'exposition</param>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exhibition = await _context.Exhibitions
                .Include(e => e.ExhibitionArtworks)
                .ThenInclude(ea => ea.Artwork)
                .FirstOrDefaultAsync(m => m.Id == id);

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
                // Save uploaded image
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/exhibitions");
                    if (!Directory.Exists(uploadsDir))
                        Directory.CreateDirectory(uploadsDir);
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploadsDir, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    exhibition.ImagePath = "/images/exhibitions/" + fileName;
                }
                _context.Add(exhibition);
                await _context.SaveChangesAsync();
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
            if (id == null)
            {
                return NotFound();
            }

            var exhibition = await _context.Exhibitions.FindAsync(id);
            if (exhibition == null)
            {
                return NotFound();
            }
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
                    var existingExhibition = await _context.Exhibitions.FindAsync(id);
                    if (existingExhibition == null)
                        return NotFound();

                    // Update fields
                    existingExhibition.Title = exhibition.Title;
                    existingExhibition.Description = exhibition.Description;
                    existingExhibition.StartDate = exhibition.StartDate;
                    existingExhibition.EndDate = exhibition.EndDate;
                    existingExhibition.Location = exhibition.Location;

                    // Handle image change
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/exhibitions");
                        if (!Directory.Exists(uploadsDir))
                            Directory.CreateDirectory(uploadsDir);
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(uploadsDir, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        existingExhibition.ImagePath = "/images/exhibitions/" + fileName;
                    }

                    _context.Update(existingExhibition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExhibitionExists(exhibition.Id))
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
            if (id == null)
            {
                return NotFound();
            }

            var exhibition = await _context.Exhibitions
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var exhibition = await _context.Exhibitions.FindAsync(id);
            if (exhibition == null)
            {
                return NotFound();
            }
            _context.Exhibitions.Remove(exhibition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

                /// <summary>
        /// Vérifie si une exposition existe dans la base de données.
        /// </summary>
        /// <param name="id">Identifiant de l'exposition</param>
        /// <returns>Vrai si l'exposition existe, faux sinon</returns>
        private bool ExhibitionExists(int id)
        {
            return _context.Exhibitions.Any(e => e.Id == id);
        }
    }
} 