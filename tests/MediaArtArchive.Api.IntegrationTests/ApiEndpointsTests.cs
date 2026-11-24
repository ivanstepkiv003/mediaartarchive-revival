using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace MediaArtArchive.Api.IntegrationTests;

public sealed class ApiEndpointsTests(ApiWebApplicationFactory factory)
    : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Api_root_returns_version_information()
    {
        var response = await _client.GetAsync("/api/v1");

        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<ApiInformationResponse>();
        Assert.Equal("Media Art Archive API", payload?.Name);
        Assert.Equal("v1", payload?.Version);
    }

    [Fact]
    public async Task Liveness_endpoint_is_healthy_without_external_dependencies()
    {
        var response = await _client.GetAsync("/alive");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Healthy", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task OpenApi_contains_versioned_and_health_routes()
    {
        var response = await _client.GetAsync("/openapi/v1.json");

        response.EnsureSuccessStatusCode();
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var paths = document.RootElement.GetProperty("paths");
        Assert.True(paths.TryGetProperty("/api/v1", out _));
        Assert.True(paths.TryGetProperty("/health", out _));
        Assert.True(paths.TryGetProperty("/alive", out _));
        Assert.True(paths.TryGetProperty("/api/v1/artists", out _));
        Assert.True(paths.TryGetProperty("/api/v1/works", out _));
        Assert.True(paths.TryGetProperty("/api/v1/publications", out _));
        Assert.True(paths.TryGetProperty("/api/v1/auth/login/google", out _));
    }

    [Fact]
    public async Task Signed_out_admin_session_returns_unauthorized()
    {
        var response = await _client.GetAsync("/api/v1/auth/me");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Signed_out_write_request_returns_unauthorized()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/artists", new { displayName = "Test artist" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Authenticated_write_without_antiforgery_token_is_rejected()
    {
        using var client = factory.CreateAuthenticatedClient();
        var response = await client.PostAsJsonAsync("/api/v1/artists", new { displayName = "Test artist" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private sealed record ApiInformationResponse(string Name, string Version);
}
