# Project documentation

This directory contains the living technical documentation for the cultural
platform and the revived Ukrainian Media Art Archive.

## Contents

- [Local development](development.md) — prerequisites, configuration, VS Code
  startup instructions, and verification commands.
- [Authentication and administration](admin.md) — Google OAuth, user sessions,
  administrator access, and local secret setup.
- [Public interface design guidelines](design-guidelines.md) — the canonical
  information-first visual direction and review checklist for frontend changes.
- [Architecture](architecture.md) — the initial application shape and boundaries.
- [Data model](data-model.md) — the proposed domain entities and relationships.
- [Database decision](decisions/0001-primary-database.md) — PostgreSQL versus MongoDB.
- [Backlog](backlog/README.md) — ordered work items for the next milestones.

Documentation should be updated in the same change as any implementation that
alters a documented contract or decision.
