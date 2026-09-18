# 005: Article entity and endpoints

## Goal

Persist editorial articles, including articles that belong to a publication.

## Scope

- Define the `Article` entity, migration, and API contracts.
- Allow an article to reference a publication while also supporting standalone
  articles.
- Add create, read, update, delete, and paginated list operations.
- Support filtering by publication, category, language, and publication date.
- Start with a byline and record contributor modeling as follow-up work unless
  authorship requirements are ready.

## Acceptance criteria

- Standalone and publication-linked articles can be managed through the API.
- A publication link cannot reference a missing publication.
- Article body and language validation are explicit and tested.
- List filtering and pagination are covered by integration tests.
