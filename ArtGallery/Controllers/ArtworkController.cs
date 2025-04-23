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

        public ArtworkController(ApplicationDbContext context)
        {
            _context = context;
        }

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

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

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

        private bool ArtworkExists(int id)
        {
            return _context.Artworks.Any(e => e.Id == id);
        }
    }
} 