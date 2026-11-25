using MediaArtArchive.Api.Domain;
using MediaArtArchive.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediaArtArchive.Api.Endpoints;

public static class PublicationEndpoints
{
    public static RouteGroupBuilder MapPublicationEndpoints(this RouteGroupBuilder api)
    {
        var publications = api.MapGroup("/publications").WithTags("Publications");

        publications.MapGet("/", async (ArchiveDbContext db, CancellationToken ct) =>
        {
            var records = await db.Publications.AsNoTracking().OrderBy(x => x.Title).ToListAsync(ct);
            return records.Select(x => x.ToResponse());
        });

        publications.MapGet("/{id:guid}", async (Guid id, ArchiveDbContext db, CancellationToken ct) =>
        {
            var publication = await db.Publications.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
            return publication is null ? Results.NotFound() : Results.Ok(publication.ToResponse());
        });

        publications.MapPost("/", async (PublicationRequest request, ArchiveDbContext db, CancellationToken ct) =>
        {
            var errors = Validate(request);
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var publication = request.ApplyTo(new Publication { Title = request.Title.Trim(), Type = request.Type.Trim() });
            db.Publications.Add(publication);
            await db.SaveChangesAsync(ct);
            return Results.Created($"{ApiMetadata.RoutePrefix}/publications/{publication.Id}", publication.ToResponse());
        }).RequireAdminMutation();

        publications.MapPut("/{id:guid}", async (Guid id, PublicationRequest request, ArchiveDbContext db, CancellationToken ct) =>
        {
            var errors = Validate(request);
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var publication = await db.Publications.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (publication is null) return Results.NotFound();
            request.ApplyTo(publication);
            publication.UpdatedAt = DateTimeOffset.UtcNow;
            await db.SaveChangesAsync(ct);
            return Results.Ok(publication.ToResponse());
        }).RequireAdminMutation();

        publications.MapDelete("/{id:guid}", async (Guid id, ArchiveDbContext db, CancellationToken ct) =>
        {
            var publication = await db.Publications.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (publication is null) return Results.NotFound();
            db.Publications.Remove(publication);
            await db.SaveChangesAsync(ct);
            return Results.NoContent();
        }).RequireAdminMutation();

        return api;
    }

    private static Dictionary<string, string[]> Validate(PublicationRequest request)
    {
        var errors = EndpointValidation.Required("title", request.Title, 500);
        foreach (var error in EndpointValidation.Required("type", request.Type, 120)) errors[error.Key] = error.Value;
        EndpointValidation.Optional(errors, "publisher", request.Publisher, 240);
        EndpointValidation.Optional(errors, "isbn", request.Isbn, 32);
        EndpointValidation.Optional(errors, "issn", request.Issn, 32);
        EndpointValidation.Optional(errors, "description", request.Description, 20_000);
        EndpointValidation.Tags(errors, request.Tags);
        return errors;
    }

    private static Publication ApplyTo(this PublicationRequest request, Publication publication)
    {
        publication.Title = request.Title.Trim();
        publication.Type = request.Type.Trim();
        publication.Publisher = EndpointValidation.Clean(request.Publisher);
        publication.PublicationDate = request.PublicationDate;
        publication.Isbn = EndpointValidation.Clean(request.Isbn);
        publication.Issn = EndpointValidation.Clean(request.Issn);
        publication.Description = EndpointValidation.Clean(request.Description);
        publication.Tags = EndpointValidation.Tags([], request.Tags);
        return publication;
    }

    private static PublicationResponse ToResponse(this Publication publication) => new(
        publication.Id, publication.Title, publication.Type, publication.Publisher,
        publication.PublicationDate, publication.Isbn, publication.Issn, publication.Description, publication.Tags,
        publication.CreatedAt, publication.UpdatedAt);
}

public sealed record PublicationRequest(string Title, string Type, string? Publisher, DateOnly? PublicationDate, string? Isbn, string? Issn, string? Description,
    IReadOnlyCollection<string>? Tags = null);
public sealed record PublicationResponse(Guid Id, string Title, string Type, string? Publisher, DateOnly? PublicationDate, string? Isbn, string? Issn,
    string? Description, IReadOnlyCollection<string> Tags, DateTimeOffset CreatedAt, DateTimeOffset UpdatedAt);
