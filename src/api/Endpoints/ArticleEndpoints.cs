using MediaArtArchive.Api.Authentication;
using MediaArtArchive.Api.Domain;
using MediaArtArchive.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediaArtArchive.Api.Endpoints;

public static class ArticleEndpoints
{
    public static RouteGroupBuilder MapArticleEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/articles").WithTags("Articles");
        group.MapGet("/", async (ArchiveDbContext db, CancellationToken ct) =>
            await db.Articles.AsNoTracking().OrderByDescending(x => x.PublishedAt).ToListAsync(ct));
        group.MapGet("/{id:guid}", async (Guid id, ArchiveDbContext db, CancellationToken ct) =>
        {
            var item = await db.Articles.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
            return item is null ? Results.NotFound() : Results.Ok(item);
        });
        group.MapPost("/", async (ArticleRequest request, ArchiveDbContext db, CancellationToken ct) =>
        {
            var errors = Validate(request);
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var item = request.ApplyTo(new Article { Title = request.Title.Trim(), Byline = request.Byline.Trim(), Category = request.Category.Trim(), Excerpt = request.Excerpt.Trim() });
            db.Articles.Add(item);
            await db.SaveChangesAsync(ct);
            return Results.Created($"{ApiMetadata.RoutePrefix}/articles/{item.Id}", item);
        }).RequireAdminMutation();
        group.MapPut("/{id:guid}", async (Guid id, ArticleRequest request, ArchiveDbContext db, CancellationToken ct) =>
        {
            var errors = Validate(request);
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var item = await db.Articles.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (item is null) return Results.NotFound();
            request.ApplyTo(item);
            item.UpdatedAt = DateTimeOffset.UtcNow;
            await db.SaveChangesAsync(ct);
            return Results.Ok(item);
        }).RequireAdminMutation();
        group.MapDelete("/{id:guid}", async (Guid id, ArchiveDbContext db, CancellationToken ct) =>
        {
            var item = await db.Articles.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (item is null) return Results.NotFound();
            db.Articles.Remove(item);
            await db.SaveChangesAsync(ct);
            return Results.NoContent();
        }).RequireAdminMutation();
        return api;
    }

    private static Dictionary<string, string[]> Validate(ArticleRequest request)
    {
        var errors = EndpointValidation.Required("title", request.Title, 500);
        EndpointValidation.Optional(errors, "titleUk", request.TitleUk, 500);
        EndpointValidation.Required(errors, "byline", request.Byline, 240);
        EndpointValidation.Optional(errors, "bylineUk", request.BylineUk, 240);
        EndpointValidation.Required(errors, "category", request.Category, 120);
        EndpointValidation.Optional(errors, "categoryUk", request.CategoryUk, 120);
        EndpointValidation.Required(errors, "excerpt", request.Excerpt, 5_000);
        EndpointValidation.Optional(errors, "excerptUk", request.ExcerptUk, 5_000);
        EndpointValidation.ImageUrl(errors, request.ImageUrl);
        EndpointValidation.Tags(errors, request.Tags);
        return errors;
    }

    private static Article ApplyTo(this ArticleRequest request, Article item)
    {
        item.Title = request.Title.Trim();
        item.TitleUk = EndpointValidation.Clean(request.TitleUk);
        item.Byline = request.Byline.Trim();
        item.BylineUk = EndpointValidation.Clean(request.BylineUk);
        item.PublishedAt = request.PublishedAt;
        item.Category = request.Category.Trim();
        item.CategoryUk = EndpointValidation.Clean(request.CategoryUk);
        item.Excerpt = request.Excerpt.Trim();
        item.ExcerptUk = EndpointValidation.Clean(request.ExcerptUk);
        item.ImageUrl = EndpointValidation.Clean(request.ImageUrl);
        item.Tags = EndpointValidation.Tags([], request.Tags);
        return item;
    }
}

public sealed record ArticleRequest(string Title, string? TitleUk, string Byline, string? BylineUk,
    DateOnly PublishedAt, string Category, string? CategoryUk, string Excerpt, string? ExcerptUk, string? ImageUrl,
    IReadOnlyCollection<string>? Tags = null);
