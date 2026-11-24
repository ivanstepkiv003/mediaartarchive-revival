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

        auth.MapGet("/login/google", (GoogleConfiguration google, IConfiguration configuration) =>
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
                    RedirectUri = $"{clientUrl}/admin"
                };
                return Results.Challenge(properties, [GoogleDefaults.AuthenticationScheme]);
            })
            .WithName("LoginWithGoogle");

        auth.MapGet("/me", (ClaimsPrincipal user) =>
            user.Identity?.IsAuthenticated == true
                ? Results.Ok(new AdminSession(
                    user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
                    user.FindFirstValue(ClaimTypes.Name) ?? user.Identity.Name ?? string.Empty,
                    user.FindFirstValue(ClaimTypes.Email) ?? string.Empty))
                : Results.Unauthorized())
            .WithName("GetAdminSession")
            .Produces<AdminSession>()
            .Produces(StatusCodes.Status401Unauthorized);

        auth.MapGet("/antiforgery", (IAntiforgery antiforgery, HttpContext context) =>
            {
                var tokens = antiforgery.GetAndStoreTokens(context);
                return Results.Ok(new AntiforgeryToken(tokens.RequestToken!));
            })
            .RequireAuthorization(AdminAuthentication.Policy)
            .WithName("GetAdminAntiforgeryToken")
            .Produces<AntiforgeryToken>();

        auth.MapPost("/logout", async (HttpContext context) =>
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Results.NoContent();
            })
            .RequireAuthorization(AdminAuthentication.Policy)
            .AddEndpointFilter<ValidateAntiforgeryFilter>()
            .WithName("LogoutAdmin");

        return api;
    }
}

public sealed record AdminSession(string Id, string Name, string Email);
public sealed record AntiforgeryToken(string Token);
