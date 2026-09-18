# Proposed data model

This is the initial conceptual model. Field-level schemas will be refined in
the relevant backlog item before migrations are created.

## Core entities

### Artist

A person or collective responsible for one or more works.

Initial fields: identifier, display name, sort name, biography, location,
active years, website, and created/updated timestamps.

### Work

An artwork or other archived creative work. `Work` is the backend domain name
for the frontend's current `Artwork` model.

Initial fields: identifier, title, creation year or date, medium, description,
city, context, and created/updated timestamps.

A work can have multiple artists and an artist can have multiple works. The
relationship should therefore use an explicit `WorkArtist` join record, which
can later include role and ordering.

### Publication

A publication or issue that groups editorial material, such as a book,
catalogue, journal, or magazine issue.

Initial fields: identifier, title, publication type, publisher, publication
date, ISBN/ISSN when applicable, description, and created/updated timestamps.

### Article

An editorial text that can appear independently or within a publication.

Initial fields: identifier, title, excerpt, body, publication date, category,
language, publication identifier when applicable, and created/updated
timestamps.

Article authors should eventually be modeled as contributors rather than a
single free-text string. Until contributor requirements are defined, a simple
byline can be used in the first iteration.

## Likely supporting entities

- `MediaAsset` for image, video, audio, and document metadata.
- `Event` for exhibitions, screenings, talks, and other dated activities.
- `NewsItem` for short platform announcements.
- `Tag` for controlled or editorial classification across content types.
- `Contributor` for authors, editors, translators, photographers, and similar roles.

## Relationships

```text
Artist >---< Work
Work   >---< MediaAsset
Publication 1---* Article
Article >---< Contributor
Work/Article/Publication >---< Tag
```

Translations should not be hidden in unstructured documents. Once content
editing requirements are known, localized fields can be represented either by
dedicated translation tables or a consistent JSON structure with validation.
