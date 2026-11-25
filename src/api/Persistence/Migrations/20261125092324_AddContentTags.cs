using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaArtArchive.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddContentTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "Tags",
                schema: "archive",
                table: "works",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.AddColumn<string[]>(
                name: "Tags",
                schema: "archive",
                table: "publications",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.AddColumn<string[]>(
                name: "Tags",
                schema: "archive",
                table: "news_items",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.AddColumn<string[]>(
                name: "Tags",
                schema: "archive",
                table: "events",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.AddColumn<string[]>(
                name: "Tags",
                schema: "archive",
                table: "artists",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.AddColumn<string[]>(
                name: "Tags",
                schema: "archive",
                table: "articles",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                schema: "archive",
                table: "works");

            migrationBuilder.DropColumn(
                name: "Tags",
                schema: "archive",
                table: "publications");

            migrationBuilder.DropColumn(
                name: "Tags",
                schema: "archive",
                table: "news_items");

            migrationBuilder.DropColumn(
                name: "Tags",
                schema: "archive",
                table: "events");

            migrationBuilder.DropColumn(
                name: "Tags",
                schema: "archive",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "Tags",
                schema: "archive",
                table: "articles");
        }
    }
}
