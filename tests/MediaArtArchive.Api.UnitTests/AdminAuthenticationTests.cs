using MediaArtArchive.Api.Authentication;
using MediaArtArchive.Api.Endpoints;

namespace MediaArtArchive.Api.UnitTests;

public sealed class AdminAuthenticationTests
{
    [Theory]
    [InlineData("admin@example.com")]
    [InlineData("ADMIN@EXAMPLE.COM")]
    public void Allowlist_matching_is_case_insensitive(string email)
    {
        Assert.True(ArchiveAuthentication.IsAllowedAdminEmail(email, ["admin@example.com"]));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("other@example.com")]
    public void Unknown_email_is_not_an_administrator(string? email)
    {
        Assert.False(ArchiveAuthentication.IsAllowedAdminEmail(email, ["admin@example.com"]));
    }

    [Theory]
    [InlineData(null, "/")]
    [InlineData("", "/")]
    [InlineData("https://example.com", "/")]
    [InlineData("//example.com", "/")]
    [InlineData("/\\example.com", "/")]
    [InlineData("/archive?medium=video", "/archive?medium=video")]
    public void Login_return_url_is_restricted_to_the_client(string? returnUrl, string expected)
    {
        Assert.Equal(expected, AuthenticationEndpoints.NormalizeReturnUrl(returnUrl));
    }
}
