# Architecture

## Current state

The repository contains a React and TypeScript client under `src/client`. It
uses representative local data through `archiveService`, which is the boundary
to replace with HTTP calls when the backend is available.

The `src/api` directory is reserved for a .NET API.

## Target shape

```text
React client
    |
    | HTTPS / JSON
    v
ASP.NET Core API container
    |
    | application services and persistence
    v
PostgreSQL container + persistent volume
```

.NET Aspire defines and runs the local distributed application. The AppHost
runs on the developer machine and starts both the API image and PostgreSQL in
Docker, injects the database connection, enforces startup ordering, and exposes
the Aspire dashboard. This keeps credentials out of source control and makes
the complete backend reproducible from one command.

Binary media should not be stored directly in the primary database. Images,
video, audio, and documents should live in object storage; the database should
hold their metadata, ownership, rights information, and stable URLs or object
keys.

## Initial API conventions

- Use versioned routes under `/api/v1`.
- Exchange UTF-8 JSON.
- Use stable generated identifiers rather than titles or slugs as primary keys.
- Keep localized text explicit so Ukrainian and English content can evolve
  independently.
- Support pagination on collection endpoints from the beginning.
- Publish an OpenAPI document and use it as the client/server contract.
- Keep domain models separate from API request and response models.

Google OAuth provides user sessions through an HTTP-only cookie. Any valid
Google account permitted by the OAuth consent-screen audience can sign in, but
only identities whose email is on the administrator allowlist receive the
admin claim required by editorial CRUD endpoints. User persistence, richer
roles, review/publishing states, public/private visibility, and media storage
remain future work.
