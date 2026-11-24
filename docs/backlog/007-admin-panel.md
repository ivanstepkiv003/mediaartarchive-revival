# 007: Google-authenticated admin panel

Status: Implemented; live Google verification pending

## Goal

Give authorized administrators a focused interface for managing artists,
works, and publications without exposing write operations publicly.

## Scope

- Authenticate administrators through Google OAuth.
- Restrict access using an explicit email allowlist.
- Provide session, login, logout, and antiforgery endpoints.
- Protect all create, update, and delete API operations.
- Add responsive list, create, edit, and delete interfaces under `/admin`.
- Preserve public read access for the archive resources.

## Acceptance criteria

- An allowlisted Google user can sign in and manage all three record types.
- A signed-out request receives `401` for protected API operations.
- A Google user outside the allowlist cannot receive an admin session.
- Mutating requests without a valid antiforgery token are rejected.
- No provider secret or production credential is committed.
