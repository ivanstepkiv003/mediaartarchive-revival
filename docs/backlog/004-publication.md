# 004: Publication entity and endpoints

## Goal

Represent books, catalogues, journals, magazine issues, and other publications.

## Scope

- Refine publication types and required metadata with archive stakeholders.
- Define the `Publication` entity, migration, and API contracts.
- Add create, read, update, delete, and paginated list operations.
- Support identifiers such as ISBN or ISSN when relevant without requiring them
  for every publication type.

## Acceptance criteria

- Publications can be managed through validated API endpoints.
- Optional industry identifiers are constrained against accidental duplicates.
- Publication type and date behavior are documented and tested.
- The OpenAPI document describes all Publication operations.
