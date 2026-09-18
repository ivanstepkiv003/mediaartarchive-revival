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
ASP.NET Core API
    |
    | application services and persistence
    v
PostgreSQL
```

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

Authentication, editorial workflows, public/private visibility, and media
storage are intentionally deferred until their requirements are defined.
