using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MediaArtArchive.Api.IntegrationTests;

public sealed class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.UseSetting(
            "ConnectionStrings:archive",
            "Host=localhost;Port=1;Database=archive;Username=test;Password=test");
        builder.UseSetting("Database:ApplyMigrations", "false");
    }
}
