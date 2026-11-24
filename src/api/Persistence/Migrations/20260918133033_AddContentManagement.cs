using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediaArtArchive.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddContentManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "archive");

            migrationBuilder.CreateTable(
                name: "artists",
                schema: "archive",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    SortName = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    Biography = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: true),
                    Location = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    ActiveYears = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Website = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "publications",
                schema: "archive",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Publisher = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    PublicationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Isbn = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Issn = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Description = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "works",
                schema: "archive",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreationDate = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Medium = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(20000)", maxLength: 20000, nullable: true),
                    City = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: true),
                    Context = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_works", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "work_artists",
                schema: "archive",
                columns: table => new
                {
                    WorkId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_artists", x => new { x.WorkId, x.ArtistId });
                    table.ForeignKey(
                        name: "FK_work_artists_artists_ArtistId",
                        column: x => x.ArtistId,
                        principalSchema: "archive",
                        principalTable: "artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_work_artists_works_WorkId",
                        column: x => x.WorkId,
                        principalSchema: "archive",
                        principalTable: "works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_artists_SortName",
                schema: "archive",
                table: "artists",
                column: "SortName");

            migrationBuilder.CreateIndex(
                name: "IX_publications_Isbn",
                schema: "archive",
                table: "publications",
                column: "Isbn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_publications_Issn",
                schema: "archive",
                table: "publications",
                column: "Issn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_publications_Title",
                schema: "archive",
                table: "publications",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_work_artists_ArtistId",
                schema: "archive",
                table: "work_artists",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_works_Title",
                schema: "archive",
                table: "works",
                column: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "publications",
                schema: "archive");

            migrationBuilder.DropTable(
                name: "work_artists",
                schema: "archive");

            migrationBuilder.DropTable(
                name: "artists",
                schema: "archive");

            migrationBuilder.DropTable(
                name: "works",
                schema: "archive");
        }
    }
}
