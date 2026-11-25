/* eslint-disable react-refresh/only-export-components */
import { createContext, useCallback, useContext, useEffect, useMemo, useState, type ReactNode } from 'react'
import { useTranslation } from 'react-i18next'
import type { ArchiveEvent, Article, Artist, Artwork, NewsItem } from '../models/types'
import { adminService, type ArticleAdmin, type ArtistAdmin, type EventAdmin, type NewsAdmin, type WorkAdmin } from './adminService'

export type LatestArchiveItem = { id: string; kind: 'artwork' | 'artist' | 'article' | 'event' | 'news'; title: string; summary: string; meta: string; href: string; createdAt: string; accent?: string; imageUrl?: string; tags: string[] }
type ArchiveData = { artworks: Artwork[]; artists: Artist[]; articles: Article[]; events: ArchiveEvent[]; news: NewsItem[]; latestItems: LatestArchiveItem[]; loading: boolean; error: string | null; reload: () => Promise<void> }
const ArchiveDataContext = createContext<ArchiveData | null>(null)
export const archiveDataChangedEvent = 'archive-data-changed'
const initials = (name: string) => name.split(/\s+/).map((part) => part[0]).join('').replace(/\W/g, '').slice(0, 2).toUpperCase()

export function ArchiveDataProvider({ children }: { children: ReactNode }) {
  const { i18n } = useTranslation()
  const [raw, setRaw] = useState<{ artists: ArtistAdmin[]; works: WorkAdmin[]; articles: ArticleAdmin[]; events: EventAdmin[]; news: NewsAdmin[] }>({ artists: [], works: [], articles: [], events: [], news: [] })
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const reload = useCallback(async () => {
    setError(null)
    try {
      const [artists, works, articles, events, news] = await Promise.all([adminService.artists(), adminService.works(), adminService.articles(), adminService.events(), adminService.news()])
      setRaw({ artists, works, articles, events, news })
    } catch (reason) { setError(reason instanceof Error ? reason.message : 'Unable to load archive data.') }
    finally { setLoading(false) }
  }, [])
  useEffect(() => {
    const timer = window.setTimeout(() => { void reload() }, 0)
    return () => window.clearTimeout(timer)
  }, [reload])
  useEffect(() => {
    const listener = () => { void reload() }
    window.addEventListener(archiveDataChangedEvent, listener)
    return () => window.removeEventListener(archiveDataChangedEvent, listener)
  }, [reload])

  const value = useMemo<ArchiveData>(() => {
    const uk = i18n.resolvedLanguage?.startsWith('uk') ?? false
    const localized = (en?: string, ua?: string) => (uk ? ua : undefined) || en || ''
    const formatDate = (date: string) => new Intl.DateTimeFormat(uk ? 'uk-UA' : 'en-GB', { day: 'numeric', month: 'long', year: 'numeric', timeZone: 'UTC' }).format(new Date(`${date}T00:00:00Z`))
    const candidates: LatestArchiveItem[] = [
      ...raw.works.map((item) => { const artist = item.artists[0]; return { id: item.id, kind: 'artwork' as const, title: localized(item.title, item.titleUk), summary: localized(item.description, item.descriptionUk), meta: [localized(artist?.displayName, artist?.displayNameUk), item.creationDate].filter(Boolean).join(' · '), href: `/archive/${item.id}`, createdAt: item.createdAt ?? '', accent: item.accent, imageUrl: item.imageUrl, tags: item.tags ?? [] } }),
      ...raw.artists.map((item) => ({ id: item.id, kind: 'artist' as const, title: localized(item.displayName, item.displayNameUk), summary: localized(item.biography, item.biographyUk), meta: localized(item.location, item.locationUk), href: `/artists/${item.id}`, createdAt: item.createdAt ?? '', imageUrl: item.imageUrl, tags: item.tags ?? [] })),
      ...raw.articles.map((item) => ({ id: item.id, kind: 'article' as const, title: localized(item.title, item.titleUk), summary: localized(item.excerpt, item.excerptUk), meta: `${localized(item.byline, item.bylineUk)} · ${formatDate(item.publishedAt)}`, href: `/articles/${item.id}`, createdAt: item.createdAt ?? '', imageUrl: item.imageUrl, tags: item.tags ?? [] })),
      ...raw.events.map((item) => ({ id: item.id, kind: 'event' as const, title: localized(item.title, item.titleUk), summary: localized(item.description, item.descriptionUk), meta: `${formatDate(item.eventDate)} · ${localized(item.location, item.locationUk)}`, href: '/events', createdAt: item.createdAt ?? '', imageUrl: item.imageUrl, tags: item.tags ?? [] })),
      ...raw.news.map((item) => ({ id: item.id, kind: 'news' as const, title: localized(item.title, item.titleUk), summary: localized(item.summary, item.summaryUk), meta: formatDate(item.publishedAt), href: '/news', createdAt: item.createdAt ?? '', imageUrl: item.imageUrl, tags: item.tags ?? [] })),
    ].sort((a, b) => b.createdAt.localeCompare(a.createdAt))
    const seenKinds = new Set<LatestArchiveItem['kind']>()
    const latestItems = candidates.filter((item) => {
      if (seenKinds.has(item.kind)) return false
      seenKinds.add(item.kind)
      return true
    })
    return {
      loading, error, reload,
      latestItems,
      artists: raw.artists.map((item) => { const name = localized(item.displayName, item.displayNameUk); return { id: item.id, name, location: localized(item.location, item.locationUk), activeYears: localized(item.activeYears, item.activeYearsUk), bio: localized(item.biography, item.biographyUk), initials: initials(name), imageUrl: item.imageUrl, tags: item.tags ?? [] } }),
      artworks: raw.works.map((item) => { const artist = item.artists[0]; return { id: item.id, title: localized(item.title, item.titleUk), artistId: artist?.artistId ?? '', artist: localized(artist?.displayName, artist?.displayNameUk), year: item.creationDate ?? '', medium: localized(item.medium, item.mediumUk), city: localized(item.city, item.cityUk), context: localized(item.context, item.contextUk), description: localized(item.description, item.descriptionUk), accent: item.accent || '#9898a7', featured: item.featured, imageUrl: item.imageUrl, tags: item.tags ?? [] } }),
      articles: raw.articles.map((item) => ({ id: item.id, title: localized(item.title, item.titleUk), author: localized(item.byline, item.bylineUk), publishedAt: formatDate(item.publishedAt), category: localized(item.category, item.categoryUk), excerpt: localized(item.excerpt, item.excerptUk), imageUrl: item.imageUrl, tags: item.tags ?? [] })),
      events: raw.events.map((item) => ({ id: item.id, title: localized(item.title, item.titleUk), date: formatDate(item.eventDate), location: localized(item.location, item.locationUk), venue: localized(item.venue, item.venueUk), status: item.status, description: localized(item.description, item.descriptionUk), imageUrl: item.imageUrl, tags: item.tags ?? [] })),
      news: raw.news.map((item) => ({ id: item.id, date: formatDate(item.publishedAt), title: localized(item.title, item.titleUk), summary: localized(item.summary, item.summaryUk), imageUrl: item.imageUrl, tags: item.tags ?? [] })),
    }
  }, [error, i18n.resolvedLanguage, loading, raw, reload])
  return <ArchiveDataContext.Provider value={value}>{children}</ArchiveDataContext.Provider>
}

export function useArchiveData() {
  const value = useContext(ArchiveDataContext)
  if (!value) throw new Error('useArchiveData must be used inside ArchiveDataProvider.')
  return value
}
