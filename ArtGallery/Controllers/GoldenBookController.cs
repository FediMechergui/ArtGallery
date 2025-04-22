using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ArtGallery.Controllers
{
    public class GoldenBookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GoldenBookController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: GoldenBook
        public async Task<IActionResult> Index()
        {
            return View(await _context.GoldenBookEntries
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync());
        }

        // GET: GoldenBook/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GoldenBook/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Message")] GoldenBookEntry goldenBookEntry)
        {
            if (ModelState.IsValid)
            {
                goldenBookEntry.CreatedAt = DateTime.Now;
                _context.Add(goldenBookEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(goldenBookEntry);
        }

        // GET: GoldenBook/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goldenBookEntry = await _context.GoldenBookEntries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goldenBookEntry == null)
            {
                return NotFound();
            }

            return View(goldenBookEntry);
        }

        // POST: GoldenBook/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goldenBookEntry = await _context.GoldenBookEntries.FindAsync(id);
            if (goldenBookEntry != null)
            {
                _context.GoldenBookEntries.Remove(goldenBookEntry);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GoldenBookEntryExists(int id)
        {
            return _context.GoldenBookEntries.Any(e => e.Id == id);
        }
    }
} 