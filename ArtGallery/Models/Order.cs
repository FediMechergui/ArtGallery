using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Models
{
    // Statut d'une commande.
    public enum OrderStatus
    {
        Pending,
        Approved,
        Declined,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    // Représente une commande passée par un utilisateur.
    public class Order
    {
        // Initialise une nouvelle instance de la classe Order et la liste des détails de commande.
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

        // Identifiant unique de la commande.
        public int Id { get; set; }

        // Identifiant de l'utilisateur ayant passé la commande.
        [Required]
        public required string UserId { get; set; }

        // Référence à l'utilisateur ayant passé la commande.
        [Required]
        public required ApplicationUser User { get; set; }

        // Date de la commande.
        [Required]
        public DateTime OrderDate { get; set; }

        // Statut de la commande.
        [Required]
        public OrderStatus Status { get; set; }

        // Montant total de la commande.
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Adresse de livraison.
        [Required]
        [StringLength(500)]
        public required string ShippingAddress { get; set; }

        // Ville de livraison.
        [Required]
        [StringLength(100)]
        public required string ShippingCity { get; set; }

        // Région ou état de livraison.
        [Required]
        [StringLength(100)]
        public required string ShippingState { get; set; }

        // Code postal de livraison.
        [Required]
        [StringLength(20)]
        public required string ShippingPostalCode { get; set; }

        // Pays de livraison.
        [Required]
        [StringLength(100)]
        public required string ShippingCountry { get; set; }

        // Notes supplémentaires pour la commande (optionnel).
        [StringLength(500)]
        public string? Notes { get; set; }

        // Nom de la personne ayant approuvé la commande (optionnel).
        [StringLength(100)]
        public string? ApprovedBy { get; set; }

        // Date d'approbation de la commande (optionnel).
        public DateTime? ApprovedAt { get; set; }

        // Détails de la commande (liste des articles commandés).
        [Required]
        public required ICollection<OrderDetail> OrderDetails { get; set; }
    }
} 