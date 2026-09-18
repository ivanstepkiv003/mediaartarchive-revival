# 003: Work entity and endpoints

## Goal

Persist creative works and relate them to their artists.

## Scope

- Define the `Work` entity and its API contracts.
- Add a `WorkArtist` relationship supporting multiple artists per work and an
  optional role/order.
- Add create, read, update, delete, and paginated list operations.
- Allow filtering works by artist and basic archive fields agreed during schema
  refinement.
- Plan the mapping from the client's current `Artwork` model to `Work`.

## Acceptance criteria

- A work can be saved with one or more existing artists.
- Relationships cannot reference missing records or be duplicated.
- Work reads include enough artist information for current client views.
- CRUD, relationship, validation, and filtering behavior is integration-tested.
