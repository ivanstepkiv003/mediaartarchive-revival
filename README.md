# Cultural Platform — Working Repository

An independent platform for Ukrainian art, technology, archives, and research. Its permanent name and identity are still in development.

The Ukrainian Media Art Archive—originally created and maintained by Yanina Prudenko and Anton Lapov—is the first archival programme being revived within the platform.

## Repository structure

```text
src/
  client/   React + TypeScript frontend
  api/      ASP.NET Core API
  MediaArtArchive.AppHost/          Aspire orchestrator
  MediaArtArchive.ServiceDefaults/  Shared health and telemetry defaults
docs/       Architecture, data model, decisions, and backlog
tests/      API unit and integration tests
```

Project documentation starts at [`docs/README.md`](docs/README.md). The ordered
implementation backlog is maintained in [`docs/backlog/README.md`](docs/backlog/README.md).

## Run the API and database

With Docker running:

```bash
dotnet tool restore
dotnet run --project src/MediaArtArchive.AppHost
```

The Aspire dashboard URL printed in the terminal provides the API endpoint,
PostgreSQL status, logs, traces, and health checks. Both the API and PostgreSQL
run in Docker containers; PostgreSQL data is kept in a persistent Docker volume.

## Run the frontend

```bash
cd src/client
npm install
npm run dev
```

The current application uses clearly labelled representative mock records. Its data service boundary is ready to be replaced with calls to the future API. The interface supports Ukrainian and English through `i18next`; the selected language is persisted in the browser.
