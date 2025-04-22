using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ArtGallery.Models;

namespace ArtGallery.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ArtworkImage> ArtworkImages { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<ExhibitionArtwork> ExhibitionArtworks { get; set; }
        public DbSet<GoldenBookEntry> GoldenBookEntries { get; set; }
        public DbSet<ExternalLink> ExternalLinks { get; set; }

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