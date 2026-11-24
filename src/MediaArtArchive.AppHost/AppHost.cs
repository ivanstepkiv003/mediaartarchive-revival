var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

var archiveDatabase = postgres.AddDatabase("archive");
var googleClientId = builder.AddParameter("google-client-id");
var googleClientSecret = builder.AddParameter("google-client-secret", secret: true);
var adminEmail = builder.AddParameter("admin-email");

builder.AddDockerfile("api", "../..", "src/api/Dockerfile")
    .WithHttpEndpoint(port: 5100, targetPort: 8080, name: "http")
    .WithHttpHealthCheck("/alive")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithEnvironment("Database__ApplyMigrations", "true")
    .WithEnvironment("Authentication__Google__ClientId", googleClientId)
    .WithEnvironment("Authentication__Google__ClientSecret", googleClientSecret)
    .WithEnvironment("Authentication__AdminEmails__0", adminEmail)
    .WithReference(archiveDatabase)
    .WaitFor(archiveDatabase);

builder.Build().Run();
