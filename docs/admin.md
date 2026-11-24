# Google authentication and administration

Visitors can sign in with a Google account from the public application. The
React application also exposes a protected administration area at `/admin` for
creating, editing, and deleting artists, works, articles, events, news, and
publications.

## Security model

- Google OAuth authenticates the person and creates an HTTP-only application
  session. The application does not store Google access or refresh tokens.
- `Authentication:AdminEmails` is an explicit allowlist; a valid Google account
  that is not listed can sign in but cannot become an administrator.
- The API stores authentication in an HTTP-only cookie.
- Every create, update, delete, and logout request requires an antiforgery token.
- API credentials and administrator email addresses are supplied at runtime and
  are not committed.

Production deployments must use HTTPS. The local HTTP configuration exists only
for localhost development.

## Google Cloud configuration

In Google Cloud:

1. Create or select a project.
2. Configure the Google Auth Platform branding and audience. Use an **External**
   audience when any Google account should be able to sign in.
3. While the app is in testing mode, add every account that should test login
   to the **Test users** list.
4. Create an OAuth client with the **Web application** type.
5. Add this exact authorized redirect URI for local development:

```text
http://localhost:5100/signin-google
```

The scheme, host, port, and path must match exactly. Add the production API
callback URL separately when a production hostname is available; production
OAuth callbacks must use HTTPS. Publishing the External app allows Google
accounts outside the test-user list to sign in.

## Local secrets

Configure the Aspire parameters from the repository root:

```bash
dotnet user-secrets set "Parameters:google-client-id" "<google-client-id>" --project src/MediaArtArchive.AppHost
dotnet user-secrets set "Parameters:google-client-secret" "<google-client-secret>" --project src/MediaArtArchive.AppHost
dotnet user-secrets set "Parameters:admin-email" "<allowed-google-account>" --project src/MediaArtArchive.AppHost
```

The client ID normally ends in `.apps.googleusercontent.com`. Never paste the
client secret into source files, `appsettings.json`, or documentation.

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

After startup, use **Sign in** in the public header. Any permitted Google test
user can establish a session. Only the email configured as `admin-email` sees
the administration link and can perform write operations.

## Local content data

PostgreSQL is the single source of truth for public archive content. The public
site and administration panel use the same API records, including English and
Ukrainian fields. After an administrator creates, edits, or deletes a record,
the public site refreshes its shared archive data automatically.

The local Aspire host sets `Database:SeedMockData=true`. On the first startup
after the content migration, the API stores the representative artists, works,
articles, events, and news in PostgreSQL and records the `mock-content-v1` seed.
The seed is versioned and runs only once, so later edits and deletions are not
overwritten or recreated when the app restarts. Existing mock artists are
reused by display name to avoid duplicates.

Production environments should omit `Database:SeedMockData` or set it to
`false` unless placeholder content is explicitly required.
