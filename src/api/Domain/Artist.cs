namespace MediaArtArchive.Api.Domain;

public sealed class Artist
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string DisplayName { get; set; }
    public string? SortName { get; set; }
    public string? Biography { get; set; }
    public string? Location { get; set; }
    public string? ActiveYears { get; set; }
    public string? Website { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public List<WorkArtist> Works { get; set; } = [];
}
