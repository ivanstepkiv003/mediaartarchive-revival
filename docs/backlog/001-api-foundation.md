# 001: API foundation

Status: Implemented; live Docker verification pending

## Goal

Create the .NET API and its persistence foundation so domain features can be
added in small, testable increments.

## Scope

- Create an ASP.NET Core Web API project under `src/api`.
- Add health checks and a versioned `/api/v1` route structure.
- Generate an OpenAPI document in development and test environments.
- Add configuration for local development without committing secrets.
- Confirm [ADR 0001](../decisions/0001-primary-database.md), or replace it with
  another accepted database decision.
- Configure Entity Framework Core and the selected database.
- Add an initial migration and a local database setup using containers.
- Establish unit and integration test projects.
- Define consistent validation and error responses.

## Acceptance criteria

- The API builds and its automated tests pass from documented commands.
- A new developer can start the API and database from repository instructions.
- The health endpoint reports success when required dependencies are available.
- OpenAPI describes the health endpoint and the versioned API surface.
- No credentials or environment-specific secrets are committed.
