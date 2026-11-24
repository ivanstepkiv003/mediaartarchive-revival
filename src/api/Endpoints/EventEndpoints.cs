using MediaArtArchive.Api.Authentication;
using MediaArtArchive.Api.Domain;
using MediaArtArchive.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MediaArtArchive.Api.Endpoints;

public static class EventEndpoints
{
    public static RouteGroupBuilder MapEventEndpoints(this RouteGroupBuilder api)
    {
        var group = api.MapGroup("/events").WithTags("Events");
        group.MapGet("/", async (ArchiveDbContext db, CancellationToken ct) =>
            await db.Events.AsNoTracking().OrderBy(x => x.EventDate).ToListAsync(ct));
        group.MapGet("/{id:guid}", async (Guid id, ArchiveDbContext db, CancellationToken ct) =>
        {
            var item = await db.Events.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
            return item is null ? Results.NotFound() : Results.Ok(item);
        });
        group.MapPost("/", async (EventRequest request, ArchiveDbContext db, CancellationToken ct) =>
        {
            var errors = Validate(request);
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var item = request.ApplyTo(new ArchiveEvent { Title = request.Title.Trim(), Location = request.Location.Trim(), Venue = request.Venue.Trim(), Status = request.Status.Trim(), Description = request.Description.Trim() });
            db.Events.Add(item);
            await db.SaveChangesAsync(ct);
            return Results.Created($"{ApiMetadata.RoutePrefix}/events/{item.Id}", item);
        }).RequireAdminMutation();
        group.MapPut("/{id:guid}", async (Guid id, EventRequest request, ArchiveDbContext db, CancellationToken ct) =>
        {
            var errors = Validate(request);
            if (errors.Count > 0) return Results.ValidationProblem(errors);
            var item = await db.Events.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (item is null) return Results.NotFound();
            request.ApplyTo(item);
            item.UpdatedAt = DateTimeOffset.UtcNow;
            await db.SaveChangesAsync(ct);
            return Results.Ok(item);
        }).RequireAdminMutation();
        group.MapDelete("/{id:guid}", async (Guid id, ArchiveDbContext db, CancellationToken ct) =>
        {
            var item = await db.Events.SingleOrDefaultAsync(x => x.Id == id, ct);
            if (item is null) return Results.NotFound();
            db.Events.Remove(item);
            await db.SaveChangesAsync(ct);
            return Results.NoContent();
        }).RequireAdminMutation();
        return api;
    }

    private static Dictionary<string, string[]> Validate(EventRequest request)
    {
        var errors = EndpointValidation.Required("title", request.Title, 500);
        EndpointValidation.Optional(errors, "titleUk", request.TitleUk, 500);
        EndpointValidation.Required(errors, "location", request.Location, 240);
        EndpointValidation.Optional(errors, "locationUk", request.LocationUk, 240);
        EndpointValidation.Required(errors, "venue", request.Venue, 500);
        EndpointValidation.Optional(errors, "venueUk", request.VenueUk, 500);
        EndpointValidation.Required(errors, "description", request.Description, 5_000);
        EndpointValidation.Optional(errors, "descriptionUk", request.DescriptionUk, 5_000);
        if (request.Status is not ("Upcoming" or "Past")) errors["status"] = ["Status must be Upcoming or Past."];
        return errors;
    }

    private static ArchiveEvent ApplyTo(this EventRequest request, ArchiveEvent item)
    {
        item.Title = request.Title.Trim();
        item.TitleUk = EndpointValidation.Clean(request.TitleUk);
        item.EventDate = request.EventDate;
        item.Location = request.Location.Trim();
        item.LocationUk = EndpointValidation.Clean(request.LocationUk);
        item.Venue = request.Venue.Trim();
        item.VenueUk = EndpointValidation.Clean(request.VenueUk);
        item.Status = request.Status;
        item.Description = request.Description.Trim();
        item.DescriptionUk = EndpointValidation.Clean(request.DescriptionUk);
        return item;
    }
}

public sealed record EventRequest(string Title, string? TitleUk, DateOnly EventDate, string Location,
    string? LocationUk, string Venue, string? VenueUk, string Status, string Description, string? DescriptionUk);
