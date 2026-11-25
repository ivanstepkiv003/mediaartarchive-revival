namespace MediaArtArchive.Api.Domain;

public sealed class ArchiveEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public string? TitleUk { get; set; }
    public DateOnly EventDate { get; set; }
    public required string Location { get; set; }
    public string? LocationUk { get; set; }
    public required string Venue { get; set; }
    public string? VenueUk { get; set; }
    public required string Status { get; set; }
    public required string Description { get; set; }
    public string? DescriptionUk { get; set; }
    public string? ImageUrl { get; set; }
    public string[] Tags { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
