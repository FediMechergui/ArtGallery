using Microsoft.AspNetCore.Identity;
using ArtGallery.Models;

namespace ArtGallery.Data
{
    // Classe utilitaire pour initialiser les données de base (rôles, admin).
    public static class SeedData
    {
        // Initialise les rôles et l'utilisateur administrateur si besoin.
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            // Récupère les gestionnaires de rôles et d'utilisateurs.
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Create Admin role if it doesn't exist
            // Crée le rôle Admin s'il n'existe pas.
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Create Admin user if it doesn't exist, or ensure they're in the Admin role
            var adminEmail = "admin@artgallery.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            // Crée l'utilisateur admin s'il n'existe pas et l'ajoute au rôle Admin.
            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    Orders = new List<Order>(),
                    ShoppingCarts = new List<ShoppingCart>()
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
            // Si l'utilisateur admin existe déjà, s'assurer qu'il a le rôle Admin.
            else
            {
                var roles = await userManager.GetRolesAsync(adminUser);
                if (!roles.Contains("Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}