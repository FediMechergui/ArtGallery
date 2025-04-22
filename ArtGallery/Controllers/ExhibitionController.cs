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

        public ExhibitionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exhibition
        public async Task<IActionResult> Index()
        {
            return View(await _context.Exhibitions
                .Include(e => e.ExhibitionArtworks)
                .ThenInclude(ea => ea.Artwork)
                .ToListAsync());
        }

        // GET: Exhibition/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exhibition/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,StartDate,EndDate,Location")] Exhibition exhibition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exhibition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exhibition);
        }

        // GET: Exhibition/Edit/5
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,StartDate,EndDate,Location")] Exhibition exhibition)
        {
            if (id != exhibition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exhibition);
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

        private bool ExhibitionExists(int id)
        {
            return _context.Exhibitions.Any(e => e.Id == id);
        }
    }
} 