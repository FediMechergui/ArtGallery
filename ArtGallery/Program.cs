// Importation des dépendances nécessaires pour ASP.NET Core, Entity Framework, l’authentification et les modèles du projet
using Microsoft.EntityFrameworkCore;
using ArtGallery.Data;
using Microsoft.AspNetCore.Identity;
using ArtGallery.Models;

// Création du builder pour configurer l’application Web
// Cette section permet de configurer les services et les dépendances de l’application
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Ajout des services nécessaires pour les contrôleurs MVC et les vues Razor
builder.Services.AddControllersWithViews();

// Register custom services for DI
builder.Services.AddScoped<ArtGallery.Services.IArtworkService, ArtGallery.Services.ArtworkService>();
builder.Services.AddScoped<ArtGallery.Services.ICategoryService, ArtGallery.Services.CategoryService>();
builder.Services.AddScoped<ArtGallery.Services.IExhibitionService, ArtGallery.Services.ExhibitionService>();

// Add Entity Framework Core
// Configuration du contexte de base de données avec SQL Server
// Cette section permet de configurer la connexion à la base de données
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
// Configuration de l’authentification et gestion des rôles avec Identity
// Cette section permet de configurer l’authentification et les rôles pour les utilisateurs
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Set to false to allow immediate login
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

// Configure cookie settings
// Configuration des paramètres de cookies pour l’authentification
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

// Add Razor Pages for Identity
// Ajout des pages Razor pour l’authentification
builder.Services.AddRazorPages();

// Construction de l’application Web à partir du builder
// Cette section permet de construire l’application Web à partir des services configurés
var app = builder.Build();

// Seed admin user and roles
// Initialisation des données pour l’administrateur et les rôles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await ArtGallery.Data.SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
// Gestion des erreurs et configuration du HSTS en production
if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Redirection HTTPS et activation des fichiers statiques (CSS, JS, images)
// Configuration de la redirection HTTPS et des fichiers statiques
app.UseHttpsRedirection();
app.UseStaticFiles();

// Activation du routage pour les requêtes HTTP
// Configuration du routage pour les requêtes HTTP
app.UseRouting();

// Activation de l’authentification et de l’autorisation
// Configuration de l’authentification et de l’autorisation
app.UseAuthentication();
app.UseAuthorization();

// Définition de la route par défaut et activation des pages Razor
// Configuration de la route par défaut et des pages Razor
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Add Identity routes
// Ajout des routes pour l’authentification
app.MapRazorPages();

// Démarrage de l’application Web
// Démarrage de l’application Web
app.Run();
