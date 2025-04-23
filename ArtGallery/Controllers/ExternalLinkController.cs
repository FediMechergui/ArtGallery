using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ArtGallery.Controllers
{
    public class ExternalLinkController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExternalLinkController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExternalLink
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExternalLinks.ToListAsync());
        }

        // GET: ExternalLink/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExternalLink/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Url,Description,Type,IsActive,SortOrder")] ExternalLink externalLink)
        {
            if (ModelState.IsValid)
            {
                externalLink.CreatedAt = DateTime.Now;
                externalLink.UpdatedAt = null;
                _context.Add(externalLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(externalLink);
        }

        // GET: ExternalLink/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var externalLink = await _context.ExternalLinks.FindAsync(id);
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
                    // Get the original entity
                    var existing = await _context.ExternalLinks.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
                    if (existing == null)
                        return NotFound();
                    externalLink.CreatedAt = existing.CreatedAt; // preserve original
                    externalLink.UpdatedAt = DateTime.Now;
                    _context.Update(externalLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExternalLinkExists(externalLink.Id))
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var externalLink = await _context.ExternalLinks
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var externalLink = await _context.ExternalLinks.FindAsync(id);
            if (externalLink == null)
            {
                return NotFound();
            }
            _context.ExternalLinks.Remove(externalLink);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExternalLinkExists(int id)
        {
            return _context.ExternalLinks.Any(e => e.Id == id);
        }
    }
} 