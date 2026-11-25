using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaArtArchive.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddContentImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "archive",
                table: "works",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "archive",
                table: "news_items",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "archive",
                table: "events",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "archive",
                table: "artists",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "archive",
                table: "articles",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "archive",
                table: "works");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "archive",
                table: "news_items");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "archive",
                table: "events");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "archive",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "archive",
                table: "articles");
        }
    }
}
