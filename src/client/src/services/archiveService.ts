import { articles, artists, artworks, events, news } from '../data/mockData'
import { articleUk, artistUk, artworkUk, eventUk, newsUk } from '../data/mockDataUk'
import i18n from '../i18n'
import type { ArchiveEvent, Article, Artist, Artwork, NewsItem } from '../models/types'

function localize<T extends { id: string }>(items: T[], translations: Record<string, Partial<Omit<T, 'id'>>>): T[] {
  if (!i18n.resolvedLanguage?.startsWith('uk')) return items
  return items.map((item) => ({ ...item, ...translations[item.id] }))
}

// This small boundary can be replaced by API calls when the .NET service is ready.
export const archiveService = {
  getArtworks: () => localize<Artwork>(artworks, artworkUk),
  getArtwork: (id: string) => localize<Artwork>(artworks, artworkUk).find((artwork) => artwork.id === id),
  getArtists: () => localize<Artist>(artists, artistUk),
  getArtist: (id: string) => localize<Artist>(artists, artistUk).find((artist) => artist.id === id),
  getArticles: () => localize<Article>(articles, articleUk),
  getArticle: (id: string) => localize<Article>(articles, articleUk).find((article) => article.id === id),
  getEvents: () => localize<ArchiveEvent>(events, eventUk),
  getNews: () => localize<NewsItem>(news, newsUk),
}
