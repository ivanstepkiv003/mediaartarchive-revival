namespace MediaArtArchive.Api.Domain;

public sealed class Work
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Title { get; set; }
    public string? TitleUk { get; set; }
    public string? CreationDate { get; set; }
    public string? Medium { get; set; }
    public string? MediumUk { get; set; }
    public string? Description { get; set; }
    public string? DescriptionUk { get; set; }
    public string? City { get; set; }
    public string? CityUk { get; set; }
    public string? Context { get; set; }
    public string? ContextUk { get; set; }
    public string? Accent { get; set; }
    public bool Featured { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public List<WorkArtist> Artists { get; set; } = [];
}

public sealed class WorkArtist
{
    public Guid WorkId { get; set; }
    public required Work Work { get; set; }
    public Guid ArtistId { get; set; }
    public required Artist Artist { get; set; }
    public string? Role { get; set; }
    public int SortOrder { get; set; }
}
