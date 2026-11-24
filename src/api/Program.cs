using MediaArtArchive.Api;
using MediaArtArchive.Api.Authentication;
using MediaArtArchive.Api.Endpoints;
using MediaArtArchive.Api.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ArchiveDbContext>("archive");
builder.Services.AddArchiveAuthentication(builder.Configuration);
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    var origins = builder.Configuration.GetSection("Client:Origins").Get<string[]>()
        ?? ["http://localhost:5173"];
    policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
}));
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
});
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapDefaultEndpoints();

var api = app.MapGroup(ApiMetadata.RoutePrefix).WithTags("API v1");
api.MapGet("/", () => Results.Ok(new ApiInformation(ApiMetadata.Name, ApiMetadata.Version)))
    .WithName("GetApiInformation")
    .Produces<ApiInformation>();
api.MapAuthenticationEndpoints();
api.MapArtistEndpoints();
api.MapWorkEndpoints();
api.MapPublicationEndpoints();
api.MapArticleEndpoints();
api.MapEventEndpoints();
api.MapNewsEndpoints();

if (builder.Configuration.GetValue<bool>("Database:ApplyMigrations"))
{
    await using var scope = app.Services.CreateAsyncScope();
    var database = scope.ServiceProvider.GetRequiredService<ArchiveDbContext>();
    await database.Database.MigrateAsync();
    if (builder.Configuration.GetValue<bool>("Database:SeedMockData"))
    {
        await MockDataSeeder.SeedAsync(database);
    }
}

await app.RunAsync();

public partial class Program;
