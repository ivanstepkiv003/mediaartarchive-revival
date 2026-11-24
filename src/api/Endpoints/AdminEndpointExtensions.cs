using MediaArtArchive.Api.Authentication;
using Microsoft.AspNetCore.Antiforgery;

namespace MediaArtArchive.Api.Endpoints;

internal static class AdminEndpointExtensions
{
    public static RouteHandlerBuilder RequireAdminMutation(this RouteHandlerBuilder endpoint) =>
        endpoint.RequireAuthorization(AdminAuthentication.Policy)
            .AddEndpointFilter<ValidateAntiforgeryFilter>();
}

internal sealed class ValidateAntiforgeryFilter(IAntiforgery antiforgery) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        try
        {
            await antiforgery.ValidateRequestAsync(context.HttpContext);
        }
        catch (AntiforgeryValidationException)
        {
            return Results.Problem(
                title: "Invalid antiforgery token.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        return await next(context);
    }
}
