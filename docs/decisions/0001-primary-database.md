# ADR 0001: Primary database

- Status: Proposed
- Date: 2026-09-18

## Context

The archive needs to represent artists, works, publications, articles, media,
and the relationships among them. Metadata will evolve as source material is
catalogued, and some records may have fields that do not apply universally.

MongoDB is attractive for flexible records, but the core model is strongly
relational: artists have many works, works can have many artists and media
assets, publications contain articles, and several content types share tags and
contributors. Referential integrity and predictable joins are important for an
archive.

## Proposed decision

Use PostgreSQL as the primary database, accessed by the .NET API through Entity
Framework Core.

Use normal relational columns and tables for identifiers, relationships,
titles, dates, visibility, and other fields that need constraints or queries.
Use PostgreSQL `jsonb` only for genuinely variable source metadata after the
stable fields have been identified.

Store binary media in object storage rather than PostgreSQL.

## Why not MongoDB as the default?

MongoDB would work, but flexible documents do not remove the need to define
relationships and validation. Cross-document references, reporting, and
archive-wide filtering would move more integrity work into application code.
PostgreSQL provides the relational behavior the known model needs while still
allowing selective schema flexibility through `jsonb`.

MongoDB should be reconsidered if discovery shows that records are mostly
self-contained documents, relationships are rare, and source-specific metadata
dominates the product's queries.

## Consequences

- Schema changes are managed through reviewed migrations.
- Core relationships can be protected with foreign keys and unique constraints.
- Flexible metadata remains possible, but it must not become an unvalidated
  substitute for the domain model.
- The decision should be accepted or replaced before implementing persistence
  in the API foundation work item.
