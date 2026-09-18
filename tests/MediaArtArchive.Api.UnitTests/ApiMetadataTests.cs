using MediaArtArchive.Api;

namespace MediaArtArchive.Api.UnitTests;

public sealed class ApiMetadataTests
{
    [Fact]
    public void Versioned_route_uses_the_declared_version()
    {
        Assert.Equal($"/api/{ApiMetadata.Version}", ApiMetadata.RoutePrefix);
    }
}
