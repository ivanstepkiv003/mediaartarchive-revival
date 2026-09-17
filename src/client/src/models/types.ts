export type Artwork = {
  id: string
  title: string
  artistId: string
  artist: string
  year: string
  medium: string
  city: string
  context: string
  description: string
  accent: string
  featured?: boolean
}

export type Artist = {
  id: string
  name: string
  location: string
  activeYears: string
  bio: string
  initials: string
}

export type Article = {
  id: string
  title: string
  author: string
  publishedAt: string
  excerpt: string
  category: string
}

export type ArchiveEvent = {
  id: string
  title: string
  date: string
  location: string
  venue: string
  description: string
  status: 'Upcoming' | 'Past'
}

export type NewsItem = {
  id: string
  date: string
  title: string
  summary: string
}
