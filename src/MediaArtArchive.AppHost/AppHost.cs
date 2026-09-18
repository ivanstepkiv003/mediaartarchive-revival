var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume();

var archiveDatabase = postgres.AddDatabase("archive");

builder.AddDockerfile("api", "../..", "src/api/Dockerfile")
    .WithHttpEndpoint(targetPort: 8080, name: "http")
    .WithHttpHealthCheck("/alive")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithEnvironment("Database__ApplyMigrations", "true")
    .WithReference(archiveDatabase)
    .WaitFor(archiveDatabase);

builder.Build().Run();
