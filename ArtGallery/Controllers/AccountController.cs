using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using ArtGallery.Models;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("", "Invalid login attempt.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        // TODO: Register user
        // If success: return RedirectToAction("Login");
        // If fail: ModelState.AddModelError("", "Registration failed.");
        return View(model);
    }
}

public class LoginViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
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
