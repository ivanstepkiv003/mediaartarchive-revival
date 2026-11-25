import { apiRequest, authenticatedMutation } from './authService'

type AuditFields = { createdAt?: string; updatedAt?: string }
export type ArtistAdmin = AuditFields & { id: string; displayName: string; displayNameUk?: string; sortName?: string; sortNameUk?: string; biography?: string; biographyUk?: string; location?: string; locationUk?: string; activeYears?: string; activeYearsUk?: string; website?: string; imageUrl?: string; tags: string[] }
export type WorkAdmin = AuditFields & { id: string; title: string; titleUk?: string; creationDate?: string; medium?: string; mediumUk?: string; description?: string; descriptionUk?: string; city?: string; cityUk?: string; context?: string; contextUk?: string; accent?: string; featured: boolean; imageUrl?: string; tags: string[]; artists: { artistId: string; displayName: string; displayNameUk?: string }[] }
export type PublicationAdmin = { id: string; title: string; type: string; publisher?: string; publicationDate?: string; isbn?: string; issn?: string; description?: string; tags: string[] }
export type ArticleAdmin = AuditFields & { id: string; title: string; titleUk?: string; byline: string; bylineUk?: string; publishedAt: string; category: string; categoryUk?: string; excerpt: string; excerptUk?: string; imageUrl?: string; tags: string[] }
export type EventAdmin = AuditFields & { id: string; title: string; titleUk?: string; eventDate: string; location: string; locationUk?: string; venue: string; venueUk?: string; status: 'Upcoming' | 'Past'; description: string; descriptionUk?: string; imageUrl?: string; tags: string[] }
export type NewsAdmin = AuditFields & { id: string; publishedAt: string; title: string; titleUk?: string; summary: string; summaryUk?: string; imageUrl?: string; tags: string[] }

export const adminService = {
  artists: () => apiRequest<ArtistAdmin[]>('/artists'),
  createArtist: (body: Omit<ArtistAdmin, 'id'>) => authenticatedMutation<ArtistAdmin>('/artists', 'POST', body),
  updateArtist: (id: string, body: Omit<ArtistAdmin, 'id'>) => authenticatedMutation<ArtistAdmin>(`/artists/${id}`, 'PUT', body),
  deleteArtist: (id: string) => authenticatedMutation<void>(`/artists/${id}`, 'DELETE'),
  works: () => apiRequest<WorkAdmin[]>('/works'),
  createWork: (body: Record<string, unknown>) => authenticatedMutation<WorkAdmin>('/works', 'POST', body),
  updateWork: (id: string, body: Record<string, unknown>) => authenticatedMutation<WorkAdmin>(`/works/${id}`, 'PUT', body),
  deleteWork: (id: string) => authenticatedMutation<void>(`/works/${id}`, 'DELETE'),
  publications: () => apiRequest<PublicationAdmin[]>('/publications'),
  createPublication: (body: Omit<PublicationAdmin, 'id'>) => authenticatedMutation<PublicationAdmin>('/publications', 'POST', body),
  updatePublication: (id: string, body: Omit<PublicationAdmin, 'id'>) => authenticatedMutation<PublicationAdmin>(`/publications/${id}`, 'PUT', body),
  deletePublication: (id: string) => authenticatedMutation<void>(`/publications/${id}`, 'DELETE'),
  articles: () => apiRequest<ArticleAdmin[]>('/articles'),
  createArticle: (body: Omit<ArticleAdmin, 'id'>) => authenticatedMutation<ArticleAdmin>('/articles', 'POST', body),
  updateArticle: (id: string, body: Omit<ArticleAdmin, 'id'>) => authenticatedMutation<ArticleAdmin>(`/articles/${id}`, 'PUT', body),
  deleteArticle: (id: string) => authenticatedMutation<void>(`/articles/${id}`, 'DELETE'),
  events: () => apiRequest<EventAdmin[]>('/events'),
  createEvent: (body: Omit<EventAdmin, 'id'>) => authenticatedMutation<EventAdmin>('/events', 'POST', body),
  updateEvent: (id: string, body: Omit<EventAdmin, 'id'>) => authenticatedMutation<EventAdmin>(`/events/${id}`, 'PUT', body),
  deleteEvent: (id: string) => authenticatedMutation<void>(`/events/${id}`, 'DELETE'),
  news: () => apiRequest<NewsAdmin[]>('/news'),
  createNews: (body: Omit<NewsAdmin, 'id'>) => authenticatedMutation<NewsAdmin>('/news', 'POST', body),
  updateNews: (id: string, body: Omit<NewsAdmin, 'id'>) => authenticatedMutation<NewsAdmin>(`/news/${id}`, 'PUT', body),
  deleteNews: (id: string) => authenticatedMutation<void>(`/news/${id}`, 'DELETE'),
}
