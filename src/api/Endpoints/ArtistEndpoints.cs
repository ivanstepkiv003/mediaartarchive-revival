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
        EndpointValidation.Optional(errors, "sortName", request.SortName, 240);
        EndpointValidation.Optional(errors, "biography", request.Biography, 10_000);
        EndpointValidation.Optional(errors, "location", request.Location, 240);
        EndpointValidation.Optional(errors, "activeYears", request.ActiveYears, 120);
        EndpointValidation.Optional(errors, "website", request.Website, 2_000);
        return errors;
    }

    private static Artist ApplyTo(this ArtistRequest request, Artist artist)
    {
        artist.DisplayName = request.DisplayName.Trim();
        artist.SortName = EndpointValidation.Clean(request.SortName);
        artist.Biography = EndpointValidation.Clean(request.Biography);
        artist.Location = EndpointValidation.Clean(request.Location);
        artist.ActiveYears = EndpointValidation.Clean(request.ActiveYears);
        artist.Website = EndpointValidation.Clean(request.Website);
        return artist;
    }

    private static ArtistResponse ToResponse(this Artist artist) => new(
        artist.Id, artist.DisplayName, artist.SortName, artist.Biography, artist.Location,
        artist.ActiveYears, artist.Website, artist.CreatedAt, artist.UpdatedAt);
}

public sealed record ArtistRequest(string DisplayName, string? SortName, string? Biography, string? Location, string? ActiveYears, string? Website);
public sealed record ArtistResponse(Guid Id, string DisplayName, string? SortName, string? Biography, string? Location, string? ActiveYears, string? Website, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
