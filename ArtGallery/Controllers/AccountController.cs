using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using ArtGallery.Models;
using ArtGallery.Services;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    // Constructeur : injection du service métier
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

        /// <summary>
    /// Affiche la page de connexion.
    /// </summary>
    [HttpGet]
    public IActionResult Login() => View();

        /// <summary>
    /// Traite la soumission du formulaire de connexion.
    /// Vérifie la validité du modèle, tente de connecter l'utilisateur et redirige vers l'accueil en cas de succès.
    /// Si la connexion échoue, affiche un message d'erreur.
    /// </summary>
    /// <param name="model">Modèle de connexion contenant l'email et le mot de passe</param>
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _accountService.LoginAsync(model, ModelState);
        if (result)
        {
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }

        /// <summary>
    /// Affiche la page d'inscription.
    /// </summary>
    [HttpGet]
    public IActionResult Register() => View();

        /// <summary>
    /// Traite la soumission du formulaire d'inscription.
    /// Si le modèle est valide, tente d'enregistrer un nouvel utilisateur.
    /// Redirige vers la page de connexion en cas de succès, sinon affiche un message d'erreur.
    /// </summary>
    /// <param name="model">Modèle d'inscription avec email, mot de passe et confirmation</param>
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var result = await _accountService.RegisterAsync(model, ModelState);
        if (result)
        {
            return RedirectToAction("Login");
        }
        return View(model);
    }
}

public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}

public class RegisterViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
    [Required, DataType(DataType.Password), Compare("Password")]
    public string ConfirmPassword { get; set; }
}
