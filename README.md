# Cultural Platform — Working Repository

An independent platform for Ukrainian art, technology, archives, and research. Its permanent name and identity are still in development.

The Ukrainian Media Art Archive—originally created and maintained by Yanina Prudenko and Anton Lapov—is the first archival programme being revived within the platform.

## Repository structure

```text
src/
  client/   React + TypeScript frontend
  api/      Reserved for the future .NET API
```

## Run the frontend

```bash
cd src/client
npm install
npm run dev
```

The current application uses clearly labelled representative mock records. Its data service boundary is ready to be replaced with calls to the future API. The interface supports Ukrainian and English through `i18next`; the selected language is persisted in the browser.
