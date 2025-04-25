using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using ArtGallery.Models;
using System.Threading.Tasks;
using ArtGallery.Services;

namespace ArtGallery.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeService _homeService;

    // Constructeur : injection du logger et du service
    public HomeController(ILogger<HomeController> logger, IHomeService homeService)
    {
        _logger = logger;
        _homeService = homeService;
    }

        /// <summary>
    /// Affiche la page d'accueil avec les œuvres et expositions mises en avant.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var viewModel = await _homeService.GetHomeViewModelAsync();
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
