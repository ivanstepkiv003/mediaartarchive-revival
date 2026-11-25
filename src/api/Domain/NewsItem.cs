namespace MediaArtArchive.Api.Domain;

public sealed class NewsItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateOnly PublishedAt { get; set; }
    public required string Title { get; set; }
    public string? TitleUk { get; set; }
    public required string Summary { get; set; }
    public string? SummaryUk { get; set; }
    public string? ImageUrl { get; set; }
    public string[] Tags { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
