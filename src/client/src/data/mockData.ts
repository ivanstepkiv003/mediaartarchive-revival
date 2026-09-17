import type { ArchiveEvent, Article, Artist, Artwork, NewsItem } from '../models/types'

// All records are representative placeholders for layout evaluation, not archival claims.
export const artworks: Artwork[] = [
  { id: 'signal-garden', title: 'Signal Garden', artistId: 'olena-m', artist: 'Olena M.', year: '2018', medium: 'Generative video', city: 'Kyiv', context: 'Independent practice', description: 'A placeholder record for a moving-image work built from generative signals and fragments of an imagined urban broadcast.', accent: '#9fb4aa', featured: true },
  { id: 'black-sea-frequency', title: 'Black Sea Frequency', artistId: 'taras-k', artist: 'Taras K.', year: '2020', medium: 'Sound installation', city: 'Odesa', context: 'Research residency', description: 'A placeholder sound work considering distance, transmission, and the shifting acoustic landscape of the Black Sea coast.', accent: '#73828f', featured: true },
  { id: 'soft-archive', title: 'Soft Archive', artistId: 'ira-b', artist: 'Ira B.', year: '2022', medium: 'Interactive web work', city: 'Lviv', context: 'Online commission', description: 'An imagined browser-based archive where personal memory is reorganised through touch, repetition, and disappearance.', accent: '#b59a86', featured: true },
  { id: 'field-notes-04', title: 'Field Notes № 04', artistId: 'maksym-h', artist: 'Maksym H.', year: '2016', medium: 'Single-channel video', city: 'Kharkiv', context: 'Artist-run space', description: 'A placeholder documentary study of overlooked technical and civic infrastructures.', accent: '#8e8979' },
  { id: 'common-ground', title: 'Common Ground', artistId: 'olena-m', artist: 'Olena M.', year: '2019', medium: 'Interactive installation', city: 'Kyiv', context: 'Museum exhibition', description: 'An imagined interactive environment responding to the movement and proximity of visitors.', accent: '#b3aa91' },
  { id: 'afterimage', title: 'Afterimage', artistId: 'ira-b', artist: 'Ira B.', year: '2023', medium: 'Digital photography', city: 'Lviv', context: 'Independent practice', description: 'A placeholder photographic sequence about image persistence and mediated recollection.', accent: '#9898a7' },
]

export const artists: Artist[] = [
  { id: 'olena-m', name: 'Olena M.', location: 'Kyiv, Ukraine', activeYears: 'Active since 2014', initials: 'OM', bio: 'Placeholder biography for a multidisciplinary artist working with generative moving image, installation, and digital archives.' },
  { id: 'taras-k', name: 'Taras K.', location: 'Odesa, Ukraine', activeYears: 'Active since 2011', initials: 'TK', bio: 'Placeholder biography for an artist and researcher exploring sound, transmission, and coastal ecologies.' },
  { id: 'ira-b', name: 'Ira B.', location: 'Lviv, Ukraine', activeYears: 'Active since 2017', initials: 'IB', bio: 'Placeholder biography for an artist whose web-based and photographic practice considers memory and unstable collections.' },
  { id: 'maksym-h', name: 'Maksym H.', location: 'Kharkiv, Ukraine', activeYears: 'Active since 2009', initials: 'MH', bio: 'Placeholder biography for a moving-image artist documenting technological systems and everyday urban space.' },
]

export const articles: Article[] = [
  { id: 'reading-unstable-media', title: 'Reading Unstable Media', author: 'Kateryna S.', publishedAt: '14 May 2025', category: 'Essay', excerpt: 'A placeholder essay on preserving works whose technologies, interfaces, and contexts continue to change.' },
  { id: 'networks-of-care', title: 'Networks of Care: On Archiving Digital Practice', author: 'Andrii P.', publishedAt: '28 March 2025', category: 'Research note', excerpt: 'How can an archive record not only an artwork, but also the relationships and acts of maintenance around it?' },
  { id: 'conversation-transmission', title: 'A Conversation About Transmission', author: 'Marta D.', publishedAt: '7 February 2025', category: 'Interview', excerpt: 'A representative conversation about radio, listening, distance, and the social life of technical media.' },
]

export const events: ArchiveEvent[] = [
  { id: 'archive-assembly', title: 'Archive Assembly: Describing Media Art', date: '12 October 2026', location: 'Kyiv', venue: 'Placeholder venue', status: 'Upcoming', description: 'A working session on language, metadata, and responsible description in media art archives.' },
  { id: 'software-memory', title: 'Software as Cultural Memory', date: '21 November 2026', location: 'Online', venue: 'Public programme', status: 'Upcoming', description: 'A public conversation about conserving software-based and networked artworks.' },
  { id: 'moving-image-screening', title: 'Moving Image: Early Experiments', date: '18 April 2025', location: 'Lviv', venue: 'Placeholder institution', status: 'Past', description: 'A representative screening and discussion programme for the historical-events layout.' },
]

export const news: NewsItem[] = [
  { id: 'cataloguing-pilot', date: '2 June 2025', title: 'Cataloguing pilot begins', summary: 'The revival project begins testing a new descriptive framework with a small set of placeholder records.' },
  { id: 'research-notes', date: '19 April 2025', title: 'Research notes now available', summary: 'The first group of methodological notes has been added to the developing archive.' },
  { id: 'project-announcement', date: '3 February 2025', title: 'Archive revival project announced', summary: 'A new phase of research and technical work begins around the Ukrainian Media Art Archive.' },
]
