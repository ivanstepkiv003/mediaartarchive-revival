using MediaArtArchive.Api.Authentication;
using MediaArtArchive.Api.Domain;
using MediaArtArchive.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediaArtArchive.Api.Endpoints;

public static class ArtistEndpoints
{
    public static RouteGroupBuilder MapArtistEndpoints(this RouteGroupBuilder api)
    {
        var artists = api.MapGroup("/artists").WithTags("Artists");

        artists.MapGet("/", async (ArchiveDbContext db, CancellationToken ct) =>
        {
            var records = await db.Artists.AsNoTracking()
                .OrderBy(x => x.SortName ?? x.DisplayName).ToListAsync(ct);
            return records.Select(x => x.ToResponse());
        });

        artists.MapGet("/{id:guid}", async (Guid id, ArchiveDbContext db, CancellationToken ct) =>
        {
            var artist = await db.Artists.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
            return artist is null ? Results.NotFound() : Results.Ok(artist.ToResponse());
        });

        artists.MapPost("/", async (ArtistRequest request, ArchiveDbContext db, CancellationToken ct) =>
        {
            var errors = Validate(request);
            if (errors.Count > 0) return Results.ValidationProblem(errors);

            var artist = request.ApplyTo(new Artist { DisplayName = request.DisplayName.Trim() });
            db.Artists.Add(artist);
            await db.SaveChangesAsync(ct);
            return Results.Created($"{ApiMetadata.RoutePrefix}/artists/{artist.Id}", artist.ToResponse());
        }).RequireAdminMutation();

        artists.MapPut("/{id:guid}", async (Guid id, ArtistRequest request, ArchiveDbContext db, CancellationToken ct) =>
        {
            var errors = Validate(request);
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var artist = await db.Artists.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (artist is null) return Results.NotFound();
            request.ApplyTo(artist);
            artist.UpdatedAt = DateTimeOffset.UtcNow;
            await db.SaveChangesAsync(ct);
            return Results.Ok(artist.ToResponse());
        }).RequireAdminMutation();

        artists.MapDelete("/{id:guid}", async (Guid id, ArchiveDbContext db, CancellationToken ct) =>
        {
            var artist = await db.Artists.Include(x => x.Works).SingleOrDefaultAsync(x => x.Id == id, ct);
            if (artist is null) return Results.NotFound();
            if (artist.Works.Count > 0) return Results.Conflict(new { detail = "Remove the artist from related works before deleting it." });
            db.Artists.Remove(artist);
            await db.SaveChangesAsync(ct);
            return Results.NoContent();
        }).RequireAdminMutation();

        return api;
    }

    private static Dictionary<string, string[]> Validate(ArtistRequest request)
    {
        var errors = EndpointValidation.Required("displayName", request.DisplayName, 240);
        EndpointValidation.Optional(errors, "displayNameUk", request.DisplayNameUk, 240);
        EndpointValidation.Optional(errors, "sortName", request.SortName, 240);
        EndpointValidation.Optional(errors, "sortNameUk", request.SortNameUk, 240);
        EndpointValidation.Optional(errors, "biography", request.Biography, 10_000);
        EndpointValidation.Optional(errors, "biographyUk", request.BiographyUk, 10_000);
        EndpointValidation.Optional(errors, "location", request.Location, 240);
        EndpointValidation.Optional(errors, "locationUk", request.LocationUk, 240);
        EndpointValidation.Optional(errors, "activeYears", request.ActiveYears, 120);
        EndpointValidation.Optional(errors, "activeYearsUk", request.ActiveYearsUk, 120);
        EndpointValidation.Optional(errors, "website", request.Website, 2_000);
        EndpointValidation.ImageUrl(errors, request.ImageUrl);
        EndpointValidation.Tags(errors, request.Tags);
        return errors;
    }

    private static Artist ApplyTo(this ArtistRequest request, Artist artist)
    {
        artist.DisplayName = request.DisplayName.Trim();
        artist.DisplayNameUk = EndpointValidation.Clean(request.DisplayNameUk);
        artist.SortName = EndpointValidation.Clean(request.SortName);
        artist.SortNameUk = EndpointValidation.Clean(request.SortNameUk);
        artist.Biography = EndpointValidation.Clean(request.Biography);
        artist.BiographyUk = EndpointValidation.Clean(request.BiographyUk);
        artist.Location = EndpointValidation.Clean(request.Location);
        artist.LocationUk = EndpointValidation.Clean(request.LocationUk);
        artist.ActiveYears = EndpointValidation.Clean(request.ActiveYears);
        artist.ActiveYearsUk = EndpointValidation.Clean(request.ActiveYearsUk);
        artist.Website = EndpointValidation.Clean(request.Website);
        artist.ImageUrl = EndpointValidation.Clean(request.ImageUrl);
        artist.Tags = EndpointValidation.Tags([], request.Tags);
        return artist;
    }

    private static ArtistResponse ToResponse(this Artist artist) => new(
        artist.Id, artist.DisplayName, artist.DisplayNameUk, artist.SortName, artist.SortNameUk,
        artist.Biography, artist.BiographyUk, artist.Location, artist.LocationUk,
        artist.ActiveYears, artist.ActiveYearsUk, artist.Website, artist.ImageUrl, artist.Tags, artist.CreatedAt, artist.UpdatedAt);
}

public sealed record ArtistRequest(string DisplayName, string? DisplayNameUk, string? SortName, string? SortNameUk,
    string? Biography, string? BiographyUk, string? Location, string? LocationUk,
    string? ActiveYears, string? ActiveYearsUk, string? Website, string? ImageUrl, IReadOnlyCollection<string>? Tags = null);
public sealed record ArtistResponse(Guid Id, string DisplayName, string? DisplayNameUk, string? SortName, string? SortNameUk,
    string? Biography, string? BiographyUk, string? Location, string? LocationUk,
    string? ActiveYears, string? ActiveYearsUk, string? Website, string? ImageUrl, IReadOnlyCollection<string> Tags, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
