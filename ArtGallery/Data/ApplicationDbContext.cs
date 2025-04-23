using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Models;

namespace ArtGallery.Data
{
    // Contexte de base de données principal de l'application, gère l'accès aux entités.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Constructeur du contexte, prend les options de configuration.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Table des œuvres d'art.
        public DbSet<Artwork> Artworks { get; set; }
        // Table des catégories.
        public DbSet<Category> Categories { get; set; }
        // Table des commandes.
        public DbSet<Order> Orders { get; set; }
        // Table des détails de commande.
        public DbSet<OrderDetail> OrderDetails { get; set; }
        // Table des paniers d'achat.
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        // Table des images d'œuvres.
        public DbSet<ArtworkImage> ArtworkImages { get; set; }
        // Table des expositions.
        public DbSet<Exhibition> Exhibitions { get; set; }
        // Table des relations exposition-œuvre.
        public DbSet<ExhibitionArtwork> ExhibitionArtworks { get; set; }
        // Table des entrées du livre d'or.
        public DbSet<GoldenBookEntry> GoldenBookEntries { get; set; }
        // Table des liens externes.
        public DbSet<ExternalLink> ExternalLinks { get; set; }

        // Configure les relations entre les entités et les comportements de la base de données.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure many-to-many relationship between Artwork and Category
            builder.Entity<Artwork>()
                .HasMany(a => a.Categories)
                .WithMany(c => c.Artworks)
                .UsingEntity(j => j.ToTable("ArtworkCategories"));

            // Configure one-to-many relationship between Artwork and ArtworkImage
            builder.Entity<Artwork>()
                .HasMany(a => a.Images)
                .WithOne(i => i.Artwork)
                .HasForeignKey(i => i.ArtworkId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Order and OrderDetail
            builder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Artwork and OrderDetail
            builder.Entity<Artwork>()
                .HasMany(a => a.OrderDetails)
                .WithOne(od => od.Artwork)
                .HasForeignKey(od => od.ArtworkId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between ApplicationUser and Order
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between ApplicationUser and ShoppingCart
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.ShoppingCarts)
                .WithOne(sc => sc.User)
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure many-to-many relationship between Artwork and Exhibition
            builder.Entity<ExhibitionArtwork>()
                .HasKey(ea => new { ea.ExhibitionId, ea.ArtworkId });

            builder.Entity<ExhibitionArtwork>()
                .HasOne(ea => ea.Exhibition)
                .WithMany(e => e.ExhibitionArtworks)
                .HasForeignKey(ea => ea.ExhibitionId);

            builder.Entity<ExhibitionArtwork>()
                .HasOne(ea => ea.Artwork)
                .WithMany(a => a.ExhibitionArtworks)
                .HasForeignKey(ea => ea.ArtworkId);

            // Configure cascade delete for ArtworkImages
            builder.Entity<ArtworkImage>()
                .HasOne(ai => ai.Artwork)
                .WithMany(a => a.Images)
                .HasForeignKey(ai => ai.ArtworkId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 