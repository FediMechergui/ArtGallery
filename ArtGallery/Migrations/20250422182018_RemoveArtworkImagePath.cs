using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtGallery.Migrations
{
    /// <inheritdoc />
    public partial class RemoveArtworkImagePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Artworks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Artworks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
