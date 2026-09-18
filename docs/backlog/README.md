# Backlog

Work items are ordered by dependency. Each item should leave the application in
a working state and include tests and documentation for its public contract.

| Order | Work item | Depends on | Status |
| ---: | --- | --- | --- |
| 1 | [API foundation](001-api-foundation.md) | — | Implemented; live Docker verification pending |
| 2 | [Artist entity and endpoints](002-artist.md) | 1 | Planned |
| 3 | [Work entity and endpoints](003-work.md) | 1, 2 | Planned |
| 4 | [Publication entity and endpoints](004-publication.md) | 1 | Planned |
| 5 | [Article entity and endpoints](005-article.md) | 1, 4 | Planned |
| 6 | [Connect the client to the API](006-client-integration.md) | 2–5 | Planned |

## Future candidates

- Media assets and object storage.
- Events and news items.
- Contributors and editorial roles.
- Tags, taxonomies, and advanced search.
- Authentication, authorization, and editorial workflows.
- Data import and provenance tracking for the existing archive.
- Backups, monitoring, deployment, and operational runbooks.
