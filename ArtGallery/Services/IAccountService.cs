using ArtGallery.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace ArtGallery.Services
{
    public interface IAccountService
    {
        Task<bool> LoginAsync(LoginViewModel model, ModelStateDictionary modelState);
        Task<bool> RegisterAsync(RegisterViewModel model, ModelStateDictionary modelState);
    }
}
