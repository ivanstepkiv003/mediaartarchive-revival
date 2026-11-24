# 008: Google user login

Status: Implemented; live Google verification pending

## Goal

Allow visitors to sign in with their Google account without granting editorial
access, while preserving the administrator allowlist as a separate
authorization boundary.

## Scope

- Accept valid Google identities permitted by the OAuth consent-screen audience.
- Return a user session that explicitly reports administrator access.
- Add sign-in, account, administration, and sign-out controls to the public header.
- Keep all content mutations restricted to the administrator claim.
- Validate post-login return paths and protect sign-out with antiforgery.
- Document Google Cloud, local secret, and test-user configuration.

## Acceptance criteria

- A configured Google test user can sign in and out from the public application.
- A signed-in user outside the administrator allowlist receives `403 Forbidden`
  from every content mutation.
- An allowlisted user can enter `/admin` and manage archive content.
- An unauthenticated user receives `401 Unauthorized` from protected endpoints.
- OAuth credentials are never stored in the repository.
