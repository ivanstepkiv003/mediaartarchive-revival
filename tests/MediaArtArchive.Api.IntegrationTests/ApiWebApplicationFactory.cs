using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MediaArtArchive.Api.IntegrationTests;

public sealed class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    public HttpClient CreateAuthenticatedClient() => WithWebHostBuilder(builder =>
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

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.UseSetting(
            "ConnectionStrings:archive",
            "Host=localhost;Port=1;Database=archive;Username=test;Password=test");
        builder.UseSetting("Database:ApplyMigrations", "false");
    }
}

internal sealed class TestAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string AuthenticationScheme = "IntegrationTest";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, "test-admin"), new Claim(ClaimTypes.Email, "admin@example.test")],
            AuthenticationScheme);
        return Task.FromResult(AuthenticateResult.Success(
            new AuthenticationTicket(new ClaimsPrincipal(identity), AuthenticationScheme)));
    }
}
