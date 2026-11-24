# Administration

The React application exposes a protected administration area at `/admin` for
creating, editing, and deleting artists, works, and publications.

## Security model

- Google OAuth authenticates the person.
- `Authentication:AdminEmails` is an explicit allowlist; a valid Google account
  that is not listed cannot become an administrator.
- The API stores authentication in an HTTP-only cookie.
- Every create, update, delete, and logout request requires an antiforgery token.
- API credentials and administrator email addresses are supplied at runtime and
  are not committed.

Production deployments must use HTTPS. The local HTTP configuration exists only
for localhost development.

## Google Cloud configuration

Create an OAuth 2.0 Web application in Google Cloud and add this authorized
redirect URI for local development:

```text
http://localhost:5100/signin-google
```

Add the production API callback URL separately when a production hostname is
available.

## Local secrets

Configure the Aspire parameters from the repository root:

```bash
dotnet user-secrets set "Parameters:google-client-id" "<google-client-id>" --project src/MediaArtArchive.AppHost
dotnet user-secrets set "Parameters:google-client-secret" "<google-client-secret>" --project src/MediaArtArchive.AppHost
dotnet user-secrets set "Parameters:admin-email" "<allowed-google-account>" --project src/MediaArtArchive.AppHost
```

Start the backend and database:

```bash
dotnet run --project src/MediaArtArchive.AppHost
```

Then start the client in another terminal:

```bash
cd src/client
npm run dev
```

The client uses `http://localhost:5100` by default. Set `VITE_API_BASE_URL` when
the API uses another origin.
