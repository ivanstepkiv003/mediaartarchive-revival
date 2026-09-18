namespace MediaArtArchive.Api;

public static class ApiMetadata
{
    public const string Name = "Media Art Archive API";
    public const string Version = "v1";
    public const string RoutePrefix = "/api/v1";
}

public sealed record ApiInformation(string Name, string Version);
