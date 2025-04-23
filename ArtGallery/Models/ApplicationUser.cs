using Microsoft.AspNetCore.Identity;

namespace ArtGallery.Models
{
    // Représente un utilisateur de l'application.
    public class ApplicationUser : IdentityUser
    {
        // Prénom de l'utilisateur.
        public string? FirstName { get; set; }
        // Nom de famille de l'utilisateur.
        public string? LastName { get; set; }
        // Date de création du compte utilisateur.
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // Date de dernière connexion de l'utilisateur (optionnel).
        public DateTime? LastLogin { get; set; }

        // Liste des commandes passées par l'utilisateur.
        public required ICollection<Order> Orders { get; set; } = new List<Order>();
        // Liste des paniers d'achat de l'utilisateur.
        public required ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
    }
}