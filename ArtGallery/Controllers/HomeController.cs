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

        // Constructeur : injection du logger et du contexte de base de données
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

        /// <summary>
    /// Affiche la page d'accueil avec les œuvres et expositions mises en avant.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            FeaturedArtworks = await _context.Artworks
                .Include(a => a.Categories)
                .Include(a => a.Images)
                .Where(a => a.IsFeatured && a.IsAvailable)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(),

            ForSaleArtworks = await _context.Artworks
                .Include(a => a.Categories)
                .Include(a => a.Images)
                .Where(a => a.IsForSale && a.IsAvailable)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(),

            AvailableArtworks = await _context.Artworks
                .Include(a => a.Categories)
                .Include(a => a.Images)
                .Where(a => a.IsAvailable)
                .OrderByDescending(a => a.CreatedAt)
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

        /// <summary>
    /// Affiche la page À propos de l'artiste.
    /// </summary>
    public IActionResult About()
    {
        return View();
    }

        /// <summary>
    /// Affiche la page de contact de l'artiste.
    /// </summary>
    public IActionResult Contact()
    {
        return View();
    }

        /// <summary>
    /// Affiche la page de politique de confidentialité.
    /// </summary>
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    /// <summary>
    /// Affiche la page d'erreur avec l'identifiant de la requête.
    /// </summary>
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
