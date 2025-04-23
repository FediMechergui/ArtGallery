using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Authorization;

namespace ArtGallery.Controllers
{
    public class ArtworkController : Controller
    {
        private readonly ApplicationDbContext _context;

                // Constructeur : injection du contexte de base de données
        public ArtworkController(ApplicationDbContext context)
        {
            _context = context;
        }

                /// <summary>
        /// Affiche la liste des œuvres d'art, avec possibilité de filtrer par catégorie ou disponibilité à la vente.
        /// </summary>
        /// <param name="category">Identifiant de la catégorie (optionnel)</param>
        /// <param name="forSale">Filtre pour les œuvres en vente (optionnel)</param>
        public async Task<IActionResult> Index(int? category, bool? forSale)
        {
            var artworksQuery = _context.Artworks
                .Include(a => a.Categories)
                .Include(a => a.Images)
                .OrderByDescending(a => a.CreatedAt)
                .AsQueryable();

            if (category.HasValue)
            {
                artworksQuery = artworksQuery.Where(a => a.Categories.Any(c => c.Id == category));
            }
            if (forSale.HasValue)
            {
                artworksQuery = artworksQuery.Where(a => a.IsForSale == forSale);
            }

            var artworks = await artworksQuery.ToListAsync();
            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Categories = categories;
            return View(artworks);
        }

                /// <summary>
        /// Affiche les détails d'une œuvre d'art spécifique.
        /// </summary>
        /// <param name="id">Identifiant de l'œuvre</param>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artworks
                .Include(a => a.Categories)
                .Include(a => a.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

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
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
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
                artwork.Categories = await _context.Categories
                    .Where(c => selectedCategories.Contains(c.Id))
                    .ToListAsync();
                _context.Add(artwork);
                await _context.SaveChangesAsync();

                // Save uploaded image as ArtworkImage
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/artworks");
                    Directory.CreateDirectory(uploads);
                    var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    var artworkImage = new ArtworkImage
                    {
                        ArtworkId = artwork.Id,
                        Artwork = artwork,
                        ImagePath = "/images/artworks/" + fileName,
                        IsPrimary = true,
                        DisplayOrder = 0
                    };
                    _context.Add(artworkImage);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _context.Categories.ToList();
            return View(artwork);
        }

                /// <summary>
        /// Affiche le formulaire d'édition d'une œuvre existante (admin).
        /// </summary>
        /// <param name="id">Identifiant de l'œuvre à éditer</param>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artworks
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artwork == null)
            {
                return NotFound();
            }

            ViewBag.Categories = _context.Categories.ToList();
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
                    var existingArtwork = await _context.Artworks
                        .Include(a => a.Categories)
                        .FirstOrDefaultAsync(a => a.Id == id);

                    if (existingArtwork == null)
                    {
                        return NotFound();
                    }

                    existingArtwork.Title = artwork.Title;
                    existingArtwork.Description = artwork.Description;
                    existingArtwork.TechniqueUsed = artwork.TechniqueUsed;
                    existingArtwork.Size = artwork.Size;
                    existingArtwork.Price = artwork.Price;
                    existingArtwork.CreationDate = artwork.CreationDate;

                    existingArtwork.IsAvailable = artwork.IsAvailable;
                    existingArtwork.IsForSale = artwork.IsForSale;
                    existingArtwork.IsFeatured = artwork.IsFeatured;
                    existingArtwork.UpdatedAt = DateTime.Now;

                    existingArtwork.Categories.Clear();
                    var selectedCategoryEntities = await _context.Categories
                        .Where(c => selectedCategories.Contains(c.Id))
                        .ToListAsync();
                    foreach (var category in selectedCategoryEntities)
                    {
                        existingArtwork.Categories.Add(category);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtworkExists(artwork.Id))
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

            ViewBag.Categories = _context.Categories.ToList();
            return View(artwork);
        }

                /// <summary>
        /// Affiche la page de confirmation de suppression d'une œuvre (admin).
        /// </summary>
        /// <param name="id">Identifiant de l'œuvre à supprimer</param>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artwork = await _context.Artworks
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(m => m.Id == id);

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
            var artwork = await _context.Artworks.FindAsync(id);
            if (artwork == null)
            {
                return NotFound();
            }

            _context.Artworks.Remove(artwork);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

                /// <summary>
        /// Vérifie si une œuvre existe dans la base de données.
        /// </summary>
        /// <param name="id">Identifiant de l'œuvre</param>
        /// <returns>Vrai si l'œuvre existe, faux sinon</returns>
        private bool ArtworkExists(int id)
        {
            return _context.Artworks.Any(e => e.Id == id);
        }
    }
} 