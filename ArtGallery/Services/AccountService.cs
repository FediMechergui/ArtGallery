using ArtGallery.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace ArtGallery.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> LoginAsync(LoginViewModel model, ModelStateDictionary modelState)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return true;
            }
            modelState.AddModelError("", "Tentative de connexion invalide.");
            return false;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model, ModelStateDictionary modelState)
        {
            var user = new ApplicationUser {
    UserName = model.Email,
    Email = model.Email,
    Orders = new List<Order>(),
    ShoppingCarts = new List<ShoppingCart>()
};
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return true;
            }
            foreach (var error in result.Errors)
            {
                modelState.AddModelError("", error.Description);
            }
            return false;
        }
    }
}
