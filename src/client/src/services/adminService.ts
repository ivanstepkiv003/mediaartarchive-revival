import { apiRequest, authenticatedMutation } from './authService'

export type ArtistAdmin = { id: string; displayName: string; sortName?: string; biography?: string; location?: string; activeYears?: string; website?: string }
export type WorkAdmin = { id: string; title: string; creationDate?: string; medium?: string; description?: string; city?: string; context?: string; artists: { artistId: string; displayName: string }[] }
export type PublicationAdmin = { id: string; title: string; type: string; publisher?: string; publicationDate?: string; isbn?: string; issn?: string; description?: string }

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
}
