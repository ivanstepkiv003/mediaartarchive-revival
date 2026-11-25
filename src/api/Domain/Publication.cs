namespace MediaArtArchive.Api.Domain;

public sealed class Publication
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public required string Type { get; set; }
    public string? Publisher { get; set; }
    public DateOnly? PublicationDate { get; set; }
    public string? Isbn { get; set; }
    public string? Issn { get; set; }
    public string? Description { get; set; }
    public string[] Tags { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
