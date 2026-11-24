# Local development

This document is the canonical guide for configuring and running the project
locally. Update it whenever the development prerequisites, configuration, or
startup commands change.

## Prerequisites

Install the following tools:

- [Visual Studio Code](https://code.visualstudio.com/)
- [.NET 10.0.301 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or a
  newer .NET 10 feature band
- [Node.js](https://nodejs.org/) with npm
- [Docker Desktop](https://www.docker.com/products/docker-desktop/), running
  before the backend is started

The C# and C# Dev Kit VS Code extensions are recommended for backend
development. VS Code's built-in TypeScript support is sufficient for the
frontend.

## Open the project

Open the repository root in VS Code. All commands below assume the integrated
terminal starts in that directory:

```powershell
code .
```

Use **Terminal > New Terminal** to create separate terminals for the backend
and frontend.

## Run the backend and database

With Docker Desktop running, execute this from the repository root:

```powershell
dotnet tool restore
dotnet run --project src/MediaArtArchive.AppHost
```

The first command restores repository-local .NET tools. The second starts the
Aspire AppHost, API, and PostgreSQL database. Open the Aspire dashboard URL
printed in the terminal to find the API endpoint and inspect resource status,
logs, traces, and health checks. PostgreSQL data is kept in a Docker volume
between runs.

The API is exposed at `http://localhost:5100` during local development.

## Run the frontend

In a second VS Code terminal, execute:

```powershell
cd src/client
npm install
npm run dev
```

`npm install` is required on the first run and whenever dependencies change.
Open the local URL printed by Vite, normally `http://localhost:5173`.

Press `Ctrl+C` in each terminal to stop the frontend and backend.

## Configuration

The frontend calls `http://localhost:5100` by default. To use a different API
origin, set `VITE_API_BASE_URL` before starting Vite. For example, in
PowerShell:

```powershell
$env:VITE_API_BASE_URL = "http://localhost:5100"
npm run dev
```

Google user authentication and administrator access require local user secrets
and Google Cloud OAuth configuration. Follow the
[authentication and administration guide](admin.md); never commit credentials
or administrator email addresses to the repository.

## Build and test

From the repository root, run the backend checks with:

```powershell
dotnet restore MediaArtArchive.slnx
dotnet build MediaArtArchive.slnx --no-restore
dotnet test MediaArtArchive.slnx --no-build
```

Run the frontend checks with:

```powershell
cd src/client
npm run lint
npm run build
```
