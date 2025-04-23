using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtGallery.Models
{
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

    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

        public int Id { get; set; }

        [Required]
        public required string UserId { get; set; }

        [Required]
        public required ApplicationUser User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(500)]
        public required string ShippingAddress { get; set; }

        [Required]
        [StringLength(100)]
        public required string ShippingCity { get; set; }

        [Required]
        [StringLength(100)]
        public required string ShippingState { get; set; }

        [Required]
        [StringLength(20)]
        public required string ShippingPostalCode { get; set; }

        [Required]
        [StringLength(100)]
        public required string ShippingCountry { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        [StringLength(100)]
        public string? ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        [Required]
        public required ICollection<OrderDetail> OrderDetails { get; set; }
    }
} 