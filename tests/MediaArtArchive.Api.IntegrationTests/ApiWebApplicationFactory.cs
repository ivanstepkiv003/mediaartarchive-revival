using System.Security.Claims;
using System.Text.Encodings.Web;
using MediaArtArchive.Api.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MediaArtArchive.Api.IntegrationTests;

public sealed class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    public HttpClient CreateAuthenticatedClient(bool isAdmin = true)
    {
        var client = WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = TestAuthenticationHandler.AuthenticationScheme;
                            options.DefaultChallengeScheme = TestAuthenticationHandler.AuthenticationScheme;
                        })
                        .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                            TestAuthenticationHandler.AuthenticationScheme,
                            _ => { });
                });
            }).CreateClient();
        client.DefaultRequestHeaders.Add(TestAuthenticationHandler.AdminHeader, isAdmin.ToString());
        return client;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.UseSetting(
            "ConnectionStrings:archive",
            "Host=localhost;Port=1;Database=archive;Username=test;Password=test");
        builder.UseSetting("Database:ApplyMigrations", "false");
        builder.UseSetting("Database:SeedMockData", "false");
    }
}

internal sealed class TestAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string AuthenticationScheme = "IntegrationTest";
    public const string AdminHeader = "X-Test-Admin";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "test-user"),
            new(ClaimTypes.Name, "Test User"),
            new(ClaimTypes.Email, "user@example.test")
        };
        if (Request.Headers.TryGetValue(AdminHeader, out var isAdmin)
            && bool.TryParse(isAdmin, out var parsedIsAdmin)
            && parsedIsAdmin)
        {
            claims.Add(new Claim(ArchiveAuthentication.AdminClaimType, bool.TrueString));
        }

        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        return Task.FromResult(AuthenticateResult.Success(
            new AuthenticationTicket(new ClaimsPrincipal(identity), AuthenticationScheme)));
    }
}
