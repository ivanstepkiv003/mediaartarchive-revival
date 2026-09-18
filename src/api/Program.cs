using MediaArtArchive.Api;
using MediaArtArchive.Api.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ArchiveDbContext>("archive");
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
});
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapDefaultEndpoints();

var api = app.MapGroup(ApiMetadata.RoutePrefix).WithTags("API v1");
api.MapGet("/", () => Results.Ok(new ApiInformation(ApiMetadata.Name, ApiMetadata.Version)))
    .WithName("GetApiInformation")
    .Produces<ApiInformation>();

if (builder.Configuration.GetValue<bool>("Database:ApplyMigrations"))
{
    await using var scope = app.Services.CreateAsyncScope();
    var database = scope.ServiceProvider.GetRequiredService<ArchiveDbContext>();
    await database.Database.MigrateAsync();
}

await app.RunAsync();

public partial class Program;
