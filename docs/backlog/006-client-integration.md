# 006: Connect the client to the API

## Goal

Replace representative local archive records with data from the backend while
preserving the current user experience.

## Scope

- Add a typed HTTP implementation behind the existing client service boundary.
- Configure the API base URL by environment.
- Map API `Work` resources to the current client views, renaming the frontend
  `Artwork` model if that improves consistency.
- Add loading, empty, and recoverable error states.
- Retain mock data only where it has an explicit development or test purpose.

## Acceptance criteria

- Artist, work, publication, and article views read from the API.
- UI behavior for both Ukrainian and English remains defined.
- Network failures produce a useful state rather than a blank or crashed page.
- Client tests cover the API service boundary and key states.
