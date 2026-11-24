using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace MediaArtArchive.Api.Authentication;

public static class ArchiveAuthentication
{
    public const string AdminPolicy = "Admin";
    public const string AdminClaimType = "mediaartarchive:admin";

    public static IServiceCollection AddArchiveAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var clientId = configuration["Authentication:Google:ClientId"];
        var clientSecret = configuration["Authentication:Google:ClientSecret"];
        var clientUrl = configuration["Client:BaseUrl"]?.TrimEnd('/') ?? "http://localhost:5173";
        var adminEmails = configuration.GetSection("Authentication:AdminEmails").Get<string[]>() ?? [];

        services.AddSingleton(new GoogleConfiguration(
            IsConfigured: !string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(clientSecret)));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "mediaartarchive.auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = clientId ?? "google-client-id-not-configured";
                options.ClientSecret = clientSecret ?? "google-client-secret-not-configured";
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Events.OnCreatingTicket = context =>
                {
                    var email = context.Principal?.FindFirstValue(ClaimTypes.Email);
                    if (IsAllowedAdminEmail(email, adminEmails)
                        && context.Principal?.Identity is ClaimsIdentity identity)
                    {
                        identity.AddClaim(new Claim(AdminClaimType, bool.TrueString));
                    }

                    return Task.CompletedTask;
                };
                options.Events.OnRemoteFailure = context =>
                {
                    context.HandleResponse();
                    context.Response.Redirect($"{clientUrl}/?authError=google");
                    return Task.CompletedTask;
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy(AdminPolicy, policy =>
                policy.RequireClaim(AdminClaimType, bool.TrueString));

        return services;
    }

    public static bool IsAllowedAdminEmail(string? email, IEnumerable<string> allowedEmails) =>
        email is not null && allowedEmails.Contains(email, StringComparer.OrdinalIgnoreCase);
}

public sealed record GoogleConfiguration(bool IsConfigured);
