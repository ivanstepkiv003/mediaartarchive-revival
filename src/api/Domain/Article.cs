namespace MediaArtArchive.Api.Domain;

public sealed class Article
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public string? TitleUk { get; set; }
    public required string Byline { get; set; }
    public string? BylineUk { get; set; }
    public DateOnly PublishedAt { get; set; }
    public required string Category { get; set; }
    public string? CategoryUk { get; set; }
    public required string Excerpt { get; set; }
    public string? ExcerptUk { get; set; }
    public string? ImageUrl { get; set; }
    public string[] Tags { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
