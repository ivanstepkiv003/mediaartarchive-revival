# Public interface design guidelines

This document is the source of truth for the visual direction of the public
archive. Read it before changing public React components or styles. If an
iteration intentionally changes this direction, update this document in the
same change.

## Design intent

The interface is information-first, restrained, and deliberately simple. It
should feel related to the early public web and online archive indexes without
becoming a nostalgic imitation. The design should help people scan titles,
dates, authors, types, relationships, and descriptions before they notice the
interface itself.

The desired qualities are:

- direct, compact, and legible;
- archival rather than promotional;
- intentionally plain rather than unfinished;
- structured with lists, indexes, metadata, and thin rules;
- consistent in Ukrainian and English;
- usable with a keyboard and on narrow screens.

The administration area is outside this visual system. It should remain a
neutral, practical editing tool unless a separate admin redesign is requested.

## Visual system

### Typography

- Use system fonts. Public UI text uses `Arial, Helvetica, sans-serif`.
- Use `Courier New, Courier, monospace` for metadata, labels, counts, dates,
  and technical status text.
- A conventional reading serif such as Georgia may be used sparingly for long
  article body copy, but not for large decorative headings.
- Do not load external web fonts for the public interface.
- Keep page titles and section headings modest. Avoid display-scale typography,
  oversized hero text, and editorial magazine compositions.

### Colour and effects

- The base is black text on a white background.
- Use `#6658ff` for primary public links and active navigation.
- Use neutral greys for rules, secondary text, and placeholder imagery.
- Record-specific accent colours may appear inside small artwork previews.
- Prefer one-pixel borders and underlines.
- Avoid gradients, drop shadows, glass effects, rounded cards, pill-shaped
  controls, decorative animation, and large coloured surfaces.

### Layout and hierarchy

- The public content width is approximately `1180px`, with compact page edges.
- Prefer rows, indexes, definition lists, and table-like arrangements over card
  galleries.
- The latest additions on the home page use an equal-weight panel grid with
  thin borders, compact text, and fixed-height previews. Keep the visible
  section header out of this grid.
- Put the record type, title, summary, date, creator, and other useful metadata
  ahead of decorative content.
- Show record tags as compact, text-first metadata. Use a leading `#`, allow
  wrapping, and avoid pill or badge treatments.
- Use compact vertical spacing. Sections should be separated by a thin rule and
  a clear label, not by large empty areas.
- Do not add a promotional hero or banner to the home page. It starts with the
  latest archive information.
- Do not make one item dramatically larger merely to create visual hierarchy.
  Importance should come from content and ordering.

### Links and controls

- Links should look like links: underlined and clearly coloured where useful.
- Inputs and buttons should resemble simple native web controls with square
  corners and visible borders.
- Active navigation may change link colour but should not introduce a large
  graphical treatment.
- Hover and focus states must remain obvious without moving surrounding layout.

### Images and illustrations

- Images support identification and context; they are not the primary content.
- Use small thumbnails in indexes and lists. Larger media is appropriate only
  on an individual artwork or article page.
- Use the image attached to each record. If the record has no image, use a
  simple schematic preview made from flat shapes, lines, or interface-like
  patterns.
- Avoid decorative stock imagery, large abstract gradients, and illustrations
  that take more space than the associated information.
- When real media becomes available, preserve its aspect ratio, provide useful
  alternative text, and keep metadata visible without interaction.

## Implementation conventions

- Public styling is scoped under `.app-shell` in
  `src/client/src/styles.css`; admin styles are not part of this theme.
- Reuse the existing public components (`PageHeader`, `SectionHeader`,
  `ArtworkCard`, `ArticleCard`, `ArtistCard`, `EventCard`, and `LatestGrid`)
  instead of creating visually unrelated variants.
- Archive, artist, article, news, and event collections should use the same
  information hierarchy even when their fields differ.
- Responsive layouts may collapse columns, but must preserve type, title,
  essential metadata, and access to the record.
- UI copy must use the existing `i18next` resources. Do not hardcode new public
  labels in only one language.
- Keep semantic headings, landmark elements, keyboard access, visible focus,
  and reduced-motion support.

## Review checklist

Before accepting a public interface change, verify:

1. Can a visitor identify the record type, title, and essential metadata before
   noticing the decoration?
2. Does the change use compact rows or indexes where a gallery is unnecessary?
3. Are headings restrained, with no new hero or oversized display text?
4. Are links visibly links and controls recognisable as controls?
5. Are illustrations small, useful, and subordinate to the information?
6. Does the layout work in Ukrainian and English without truncating essential
   information?
7. Does it remain usable at desktop and mobile widths?
8. Does it avoid external fonts and unnecessary visual effects?
9. If the visual direction changed, was this document updated?
