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
                x.Id, x.Title, x.CreationDate, x.Medium, x.Description, x.City, x.Context,
                x.Artists.OrderBy(a => a.SortOrder).Select(a => new WorkArtistResponse(a.ArtistId, a.Artist.DisplayName, a.Role, a.SortOrder)).ToList(),
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
        EndpointValidation.Optional(errors, "creationDate", request.CreationDate, 120);
        EndpointValidation.Optional(errors, "medium", request.Medium, 500);
        EndpointValidation.Optional(errors, "description", request.Description, 20_000);
        EndpointValidation.Optional(errors, "city", request.City, 240);
        EndpointValidation.Optional(errors, "context", request.Context, 2_000);
        var artistIds = request.ArtistIds?.Distinct().ToArray() ?? [];
        if (artistIds.Length == 0) errors["artistIds"] = ["At least one artist is required."];
        else if (await db.Artists.CountAsync(x => artistIds.Contains(x.Id), ct) != artistIds.Length)
            errors["artistIds"] = ["One or more selected artists do not exist."];
        return errors;
    }

    private static Work ApplyTo(this WorkRequest request, Work work)
    {
        work.Title = request.Title.Trim();
        work.CreationDate = EndpointValidation.Clean(request.CreationDate);
        work.Medium = EndpointValidation.Clean(request.Medium);
        work.Description = EndpointValidation.Clean(request.Description);
        work.City = EndpointValidation.Clean(request.City);
        work.Context = EndpointValidation.Clean(request.Context);
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
            x.Id, x.Title, x.CreationDate, x.Medium, x.Description, x.City, x.Context,
            x.Artists.OrderBy(a => a.SortOrder).Select(a => new WorkArtistResponse(a.ArtistId, a.Artist.DisplayName, a.Role, a.SortOrder)).ToList(),
            x.CreatedAt, x.UpdatedAt)).SingleOrDefaultAsync(ct);
}

public sealed record WorkRequest(string Title, string? CreationDate, string? Medium, string? Description, string? City, string? Context, IReadOnlyCollection<Guid>? ArtistIds);
public sealed record WorkArtistResponse(Guid ArtistId, string DisplayName, string? Role, int SortOrder);
public sealed record WorkResponse(Guid Id, string Title, string? CreationDate, string? Medium, string? Description, string? City, string? Context, IReadOnlyCollection<WorkArtistResponse> Artists, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
