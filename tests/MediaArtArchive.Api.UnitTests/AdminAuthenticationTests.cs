using MediaArtArchive.Api.Authentication;

namespace MediaArtArchive.Api.UnitTests;

public sealed class AdminAuthenticationTests
{
    [Theory]
    [InlineData("admin@example.com")]
    [InlineData("ADMIN@EXAMPLE.COM")]
    public void Allowlist_matching_is_case_insensitive(string email)
    {
        Assert.True(AdminAuthentication.IsAllowedAdminEmail(email, ["admin@example.com"]));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("other@example.com")]
    public void Unknown_email_is_not_an_administrator(string? email)
    {
        Assert.False(AdminAuthentication.IsAllowedAdminEmail(email, ["admin@example.com"]));
    }
}
