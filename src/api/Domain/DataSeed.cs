namespace MediaArtArchive.Api.Domain;

public sealed class DataSeed
{
    public required string Name { get; set; }
    public DateTimeOffset AppliedAt { get; set; } = DateTimeOffset.UtcNow;
}
