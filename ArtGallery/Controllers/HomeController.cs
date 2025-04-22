using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using System.Threading.Tasks;

namespace ArtGallery.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            FeaturedArtworks = await _context.Artworks
                .Include(a => a.Categories)
                .Include(a => a.Images)
                .Where(a => a.IsFeatured && a.IsAvailable)
                .OrderByDescending(a => a.CreatedAt)
                .Take(6)
                .ToListAsync(),

            LatestExhibitions = await _context.Exhibitions
                .Include(e => e.ExhibitionArtworks)
                .ThenInclude(ea => ea.Artwork)
                .Where(e => e.IsActive && e.EndDate >= DateTime.Now)
                .OrderBy(e => e.StartDate)
                .Take(3)
                .ToListAsync()
        };

        return View(viewModel);
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
