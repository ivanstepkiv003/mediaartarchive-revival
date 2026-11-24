using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaArtArchive.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveMockContentToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Accent",
                schema: "archive",
                table: "works",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityUk",
                schema: "archive",
                table: "works",
                type: "character varying(240)",
                maxLength: 240,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContextUk",
                schema: "archive",
                table: "works",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionUk",
                schema: "archive",
                table: "works",
                type: "character varying(20000)",
                maxLength: 20000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Featured",
                schema: "archive",
                table: "works",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MediumUk",
                schema: "archive",
                table: "works",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleUk",
                schema: "archive",
                table: "works",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActiveYearsUk",
                schema: "archive",
                table: "artists",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BiographyUk",
                schema: "archive",
                table: "artists",
                type: "character varying(10000)",
                maxLength: 10000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayNameUk",
                schema: "archive",
                table: "artists",
                type: "character varying(240)",
                maxLength: 240,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationUk",
                schema: "archive",
                table: "artists",
                type: "character varying(240)",
                maxLength: 240,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SortNameUk",
                schema: "archive",
                table: "artists",
                type: "character varying(240)",
                maxLength: 240,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "articles",
                schema: "archive",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TitleUk = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Byline = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    BylineUk = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    PublishedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    Category = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    CategoryUk = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Excerpt = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    ExcerptUk = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "data_seeds",
                schema: "archive",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    AppliedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_data_seeds", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "events",
                schema: "archive",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TitleUk = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EventDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Location = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    LocationUk = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    Venue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    VenueUk = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    DescriptionUk = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "news_items",
                schema: "archive",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PublishedAt = table.Column<DateOnly>(type: "date", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TitleUk = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Summary = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    SummaryUk = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_news_items", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_articles_PublishedAt",
                schema: "archive",
                table: "articles",
                column: "PublishedAt");

            migrationBuilder.CreateIndex(
                name: "IX_events_EventDate",
                schema: "archive",
                table: "events",
                column: "EventDate");

            migrationBuilder.CreateIndex(
                name: "IX_news_items_PublishedAt",
                schema: "archive",
                table: "news_items",
                column: "PublishedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articles",
                schema: "archive");

            migrationBuilder.DropTable(
                name: "data_seeds",
                schema: "archive");

            migrationBuilder.DropTable(
                name: "events",
                schema: "archive");

            migrationBuilder.DropTable(
                name: "news_items",
                schema: "archive");

            migrationBuilder.DropColumn(
                name: "Accent",
                schema: "archive",
                table: "works");

            migrationBuilder.DropColumn(
                name: "CityUk",
                schema: "archive",
                table: "works");

            migrationBuilder.DropColumn(
                name: "ContextUk",
                schema: "archive",
                table: "works");

            migrationBuilder.DropColumn(
                name: "DescriptionUk",
                schema: "archive",
                table: "works");

            migrationBuilder.DropColumn(
                name: "Featured",
                schema: "archive",
                table: "works");

            migrationBuilder.DropColumn(
                name: "MediumUk",
                schema: "archive",
                table: "works");

            migrationBuilder.DropColumn(
                name: "TitleUk",
                schema: "archive",
                table: "works");

            migrationBuilder.DropColumn(
                name: "ActiveYearsUk",
                schema: "archive",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "BiographyUk",
                schema: "archive",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "DisplayNameUk",
                schema: "archive",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "LocationUk",
                schema: "archive",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "SortNameUk",
                schema: "archive",
                table: "artists");
        }
    }
}
