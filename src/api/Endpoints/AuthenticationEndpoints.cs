using System.Security.Claims;
using MediaArtArchive.Api.Authentication;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace MediaArtArchive.Api.Endpoints;

public static class AuthenticationEndpoints
{
    public static RouteGroupBuilder MapAuthenticationEndpoints(this RouteGroupBuilder api)
    {
        var auth = api.MapGroup("/auth").WithTags("Authentication");

        auth.MapGet("/login/google", (string? returnUrl, GoogleConfiguration google, IConfiguration configuration) =>
            {
                if (!google.IsConfigured)
                {
                    return Results.Problem(
                        "Google authentication has not been configured.",
                        statusCode: StatusCodes.Status503ServiceUnavailable);
                }

                var clientUrl = configuration["Client:BaseUrl"]?.TrimEnd('/')
                    ?? "http://localhost:5173";
                var properties = new AuthenticationProperties
                {
                    RedirectUri = $"{clientUrl}{NormalizeReturnUrl(returnUrl)}"
                };
                return Results.Challenge(properties, [GoogleDefaults.AuthenticationScheme]);
            })
            .WithName("LoginWithGoogle");

        auth.MapGet("/me", (ClaimsPrincipal user) =>
            user.Identity?.IsAuthenticated == true
                ? Results.Ok(new UserSession(
                    user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
                    user.FindFirstValue(ClaimTypes.Name) ?? user.Identity.Name ?? string.Empty,
                    user.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
                    user.HasClaim(ArchiveAuthentication.AdminClaimType, bool.TrueString)))
                : Results.Unauthorized())
            .WithName("GetUserSession")
            .Produces<UserSession>()
            .Produces(StatusCodes.Status401Unauthorized);

        auth.MapGet("/antiforgery", (IAntiforgery antiforgery, HttpContext context) =>
            {
                var tokens = antiforgery.GetAndStoreTokens(context);
                return Results.Ok(new AntiforgeryToken(tokens.RequestToken!));
            })
            .RequireAuthorization()
            .WithName("GetUserAntiforgeryToken")
            .Produces<AntiforgeryToken>();

        auth.MapPost("/logout", async (HttpContext context) =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.NoContent();
            })
            .RequireAuthorization()
            .AddEndpointFilter<ValidateAntiforgeryFilter>()
            .WithName("LogoutUser");

        return api;
    }

    internal static string NormalizeReturnUrl(string? returnUrl) =>
        !string.IsNullOrWhiteSpace(returnUrl)
        && returnUrl.StartsWith('/')
        && !returnUrl.StartsWith("//")
        && !returnUrl.Contains('\\')
        && returnUrl.All(character => !char.IsControl(character))
            ? returnUrl
            : "/";
}

public sealed record UserSession(string Id, string Name, string Email, bool IsAdmin);
public sealed record AntiforgeryToken(string Token);
