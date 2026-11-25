using MediaArtArchive.Api.Domain;
using MediaArtArchive.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediaArtArchive.Api.Endpoints;

public static class WorkEndpoints
{
    public static RouteGroupBuilder MapWorkEndpoints(this RouteGroupBuilder api)
    {
        var works = api.MapGroup("/works").WithTags("Works");

        works.MapGet("/", async (ArchiveDbContext db, CancellationToken ct) =>
            await db.Works.AsNoTracking().OrderBy(x => x.Title).Select(x => new WorkResponse(
                x.Id, x.Title, x.TitleUk, x.CreationDate, x.Medium, x.MediumUk, x.Description, x.DescriptionUk,
                x.City, x.CityUk, x.Context, x.ContextUk, x.Accent, x.Featured, x.ImageUrl, x.Tags,
                x.Artists.OrderBy(a => a.SortOrder).Select(a => new WorkArtistResponse(a.ArtistId, a.Artist.DisplayName, a.Artist.DisplayNameUk, a.Role, a.SortOrder)).ToList(),
                x.CreatedAt, x.UpdatedAt)).ToListAsync(ct));

        works.MapGet("/{id:guid}", async (Guid id, ArchiveDbContext db, CancellationToken ct) =>
        {
            var work = await FindResponse(db, id, ct);
            return work is null ? Results.NotFound() : Results.Ok(work);
        });

        works.MapPost("/", async (WorkRequest request, ArchiveDbContext db, CancellationToken ct) =>
        {
            var errors = await Validate(request, db, ct);
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var work = request.ApplyTo(new Work { Title = request.Title.Trim() });
            ReplaceArtists(work, request.ArtistIds);
            db.Works.Add(work);
            await db.SaveChangesAsync(ct);
            return Results.Created($"{ApiMetadata.RoutePrefix}/works/{work.Id}", await FindResponse(db, work.Id, ct));
        }).RequireAdminMutation();

        works.MapPut("/{id:guid}", async (Guid id, WorkRequest request, ArchiveDbContext db, CancellationToken ct) =>
        {
            var errors = await Validate(request, db, ct);
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var work = await db.Works.Include(x => x.Artists).SingleOrDefaultAsync(x => x.Id == id, ct);
            if (work is null) return Results.NotFound();
            request.ApplyTo(work);
            ReplaceArtists(work, request.ArtistIds);
            work.UpdatedAt = DateTimeOffset.UtcNow;
            await db.SaveChangesAsync(ct);
            return Results.Ok(await FindResponse(db, work.Id, ct));
        }).RequireAdminMutation();

        works.MapDelete("/{id:guid}", async (Guid id, ArchiveDbContext db, CancellationToken ct) =>
        {
            var work = await db.Works.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (work is null) return Results.NotFound();
            db.Works.Remove(work);
            await db.SaveChangesAsync(ct);
            return Results.NoContent();
        }).RequireAdminMutation();

        return api;
    }

    private static async Task<Dictionary<string, string[]>> Validate(WorkRequest request, ArchiveDbContext db, CancellationToken ct)
    {
        var errors = EndpointValidation.Required("title", request.Title, 500);
        EndpointValidation.Optional(errors, "titleUk", request.TitleUk, 500);
        EndpointValidation.Optional(errors, "creationDate", request.CreationDate, 120);
        EndpointValidation.Optional(errors, "medium", request.Medium, 500);
        EndpointValidation.Optional(errors, "mediumUk", request.MediumUk, 500);
        EndpointValidation.Optional(errors, "description", request.Description, 20_000);
        EndpointValidation.Optional(errors, "descriptionUk", request.DescriptionUk, 20_000);
        EndpointValidation.Optional(errors, "city", request.City, 240);
        EndpointValidation.Optional(errors, "cityUk", request.CityUk, 240);
        EndpointValidation.Optional(errors, "context", request.Context, 2_000);
        EndpointValidation.Optional(errors, "contextUk", request.ContextUk, 2_000);
        EndpointValidation.Optional(errors, "accent", request.Accent, 20);
        EndpointValidation.ImageUrl(errors, request.ImageUrl);
        EndpointValidation.Tags(errors, request.Tags);
        var artistIds = request.ArtistIds?.Distinct().ToArray() ?? [];
        if (artistIds.Length == 0) errors["artistIds"] = ["At least one artist is required."];
        else if (await db.Artists.CountAsync(x => artistIds.Contains(x.Id), ct) != artistIds.Length)
            errors["artistIds"] = ["One or more selected artists do not exist."];
        return errors;
    }

    private static Work ApplyTo(this WorkRequest request, Work work)
    {
        work.Title = request.Title.Trim();
        work.TitleUk = EndpointValidation.Clean(request.TitleUk);
        work.CreationDate = EndpointValidation.Clean(request.CreationDate);
        work.Medium = EndpointValidation.Clean(request.Medium);
        work.MediumUk = EndpointValidation.Clean(request.MediumUk);
        work.Description = EndpointValidation.Clean(request.Description);
        work.DescriptionUk = EndpointValidation.Clean(request.DescriptionUk);
        work.City = EndpointValidation.Clean(request.City);
        work.CityUk = EndpointValidation.Clean(request.CityUk);
        work.Context = EndpointValidation.Clean(request.Context);
        work.ContextUk = EndpointValidation.Clean(request.ContextUk);
        work.Accent = EndpointValidation.Clean(request.Accent);
        work.Featured = request.Featured;
        work.ImageUrl = EndpointValidation.Clean(request.ImageUrl);
        work.Tags = EndpointValidation.Tags([], request.Tags);
        return work;
    }

    private static void ReplaceArtists(Work work, IReadOnlyCollection<Guid>? artistIds)
    {
        work.Artists.Clear();
        work.Artists.AddRange((artistIds ?? []).Distinct().Select((artistId, index) => new WorkArtist
        {
            Work = work,
            WorkId = work.Id,
            Artist = null!,
            ArtistId = artistId,
            SortOrder = index
        }));
    }

    private static Task<WorkResponse?> FindResponse(ArchiveDbContext db, Guid id, CancellationToken ct) =>
        db.Works.AsNoTracking().Where(x => x.Id == id).Select(x => new WorkResponse(
            x.Id, x.Title, x.TitleUk, x.CreationDate, x.Medium, x.MediumUk, x.Description, x.DescriptionUk,
            x.City, x.CityUk, x.Context, x.ContextUk, x.Accent, x.Featured, x.ImageUrl, x.Tags,
            x.Artists.OrderBy(a => a.SortOrder).Select(a => new WorkArtistResponse(a.ArtistId, a.Artist.DisplayName, a.Artist.DisplayNameUk, a.Role, a.SortOrder)).ToList(),
            x.CreatedAt, x.UpdatedAt)).SingleOrDefaultAsync(ct);
}

public sealed record WorkRequest(string Title, string? TitleUk, string? CreationDate, string? Medium, string? MediumUk,
    string? Description, string? DescriptionUk, string? City, string? CityUk, string? Context, string? ContextUk,
    string? Accent, bool Featured, string? ImageUrl, IReadOnlyCollection<Guid>? ArtistIds, IReadOnlyCollection<string>? Tags = null);
public sealed record WorkArtistResponse(Guid ArtistId, string DisplayName, string? DisplayNameUk, string? Role, int SortOrder);
public sealed record WorkResponse(Guid Id, string Title, string? TitleUk, string? CreationDate, string? Medium, string? MediumUk,
    string? Description, string? DescriptionUk, string? City, string? CityUk, string? Context, string? ContextUk,
    string? Accent, bool Featured, string? ImageUrl, IReadOnlyCollection<string> Tags, IReadOnlyCollection<WorkArtistResponse> Artists, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
