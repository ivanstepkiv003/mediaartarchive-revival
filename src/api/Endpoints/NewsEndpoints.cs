using MediaArtArchive.Api.Authentication;
using MediaArtArchive.Api.Domain;
using MediaArtArchive.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediaArtArchive.Api.Endpoints;

public static class NewsEndpoints
{
    public static RouteGroupBuilder MapNewsEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/news").WithTags("News");
        group.MapGet("/", async (ArchiveDbContext db, CancellationToken ct) =>
            await db.NewsItems.AsNoTracking().OrderByDescending(x => x.PublishedAt).ToListAsync(ct));
        group.MapGet("/{id:guid}", async (Guid id, ArchiveDbContext db, CancellationToken ct) =>
        {
            var item = await db.NewsItems.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
            return item is null ? Results.NotFound() : Results.Ok(item);
        });
        group.MapPost("/", async (NewsRequest request, ArchiveDbContext db, CancellationToken ct) =>
        {
            var errors = Validate(request);
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var item = request.ApplyTo(new NewsItem { Title = request.Title.Trim(), Summary = request.Summary.Trim() });
            db.NewsItems.Add(item);
            await db.SaveChangesAsync(ct);
            return Results.Created($"{ApiMetadata.RoutePrefix}/news/{item.Id}", item);
        }).RequireAdminMutation();
        group.MapPut("/{id:guid}", async (Guid id, NewsRequest request, ArchiveDbContext db, CancellationToken ct) =>
        {
            var errors = Validate(request);
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var item = await db.NewsItems.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (item is null) return Results.NotFound();
            request.ApplyTo(item);
            item.UpdatedAt = DateTimeOffset.UtcNow;
            await db.SaveChangesAsync(ct);
            return Results.Ok(item);
        }).RequireAdminMutation();
        group.MapDelete("/{id:guid}", async (Guid id, ArchiveDbContext db, CancellationToken ct) =>
        {
            var item = await db.NewsItems.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (item is null) return Results.NotFound();
            db.NewsItems.Remove(item);
            await db.SaveChangesAsync(ct);
            return Results.NoContent();
        }).RequireAdminMutation();
        return api;
    }

    private static Dictionary<string, string[]> Validate(NewsRequest request)
    {
        var errors = EndpointValidation.Required("title", request.Title, 500);
        EndpointValidation.Optional(errors, "titleUk", request.TitleUk, 500);
        EndpointValidation.Required(errors, "summary", request.Summary, 5_000);
        EndpointValidation.Optional(errors, "summaryUk", request.SummaryUk, 5_000);
        return errors;
    }

    private static NewsItem ApplyTo(this NewsRequest request, NewsItem item)
    {
        item.PublishedAt = request.PublishedAt;
        item.Title = request.Title.Trim();
        item.TitleUk = EndpointValidation.Clean(request.TitleUk);
        item.Summary = request.Summary.Trim();
        item.SummaryUk = EndpointValidation.Clean(request.SummaryUk);
        return item;
    }
}

public sealed record NewsRequest(DateOnly PublishedAt, string Title, string? TitleUk, string Summary, string? SummaryUk);
