# Media Art Archive API

ASP.NET Core API for the cultural platform and revived Ukrainian Media Art
Archive. The service is normally started through the Aspire AppHost, which runs
this API and PostgreSQL in Docker containers.

## Start the complete application

From the repository root:

```bash
dotnet tool restore
dotnet run --project src/MediaArtArchive.AppHost
```

Open the Aspire dashboard URL printed in the terminal. The dashboard lists the
API endpoint, PostgreSQL resource, logs, traces, and health state.

Docker must be running. Aspire generates the local PostgreSQL credentials and
injects the `archive` connection string into the API; no password is stored in
the repository. Database data persists in a Docker volume between AppHost runs.

## Useful endpoints

- `GET /api/v1` — API identity and supported version.
- `GET /api/v1/artists` — public artist records.
- `GET /api/v1/works` — public work records.
- `GET /api/v1/publications` — public publication records.
- `GET /api/v1/auth/login/google` — Google user sign-in.
- `GET /api/v1/auth/me` — current user session and administrator status.
- `POST /api/v1/auth/logout` — end the current user session.
- `GET /health` — readiness, including PostgreSQL connectivity.
- `GET /alive` — process liveness.
- `GET /openapi/v1.json` — OpenAPI document in Development.

## Build and test

```bash
dotnet restore MediaArtArchive.slnx
dotnet build MediaArtArchive.slnx --no-restore
dotnet test MediaArtArchive.slnx --no-build
```

## Migrations

Migrations are applied automatically when the API is started by the AppHost.
To create a migration after changing `ArchiveDbContext`:

```bash
dotnet tool restore
dotnet ef migrations add <MigrationName> --project src/api --output-dir Persistence/Migrations
```
