# 002: Artist entity and endpoints

Status: Implemented

## Goal

Persist artists and expose the first archive resource through the API.

## Scope

- Define the `Artist` domain entity and database configuration.
- Add a migration with appropriate required fields, lengths, and indexes.
- Add create, read, update, delete, and paginated list operations.
- Support lookup by stable identifier; treat any future slug as mutable metadata.
- Map API contracts independently from the persistence entity.

## Acceptance criteria

- Valid artists can be created, retrieved, listed, updated, and deleted.
- Invalid input returns the standard validation response.
- Unknown identifiers return `404`.
- Integration tests cover persistence and the public endpoints.
- The OpenAPI contract includes the Artist operations and schemas.
