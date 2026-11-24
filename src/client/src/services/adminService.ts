const apiBaseUrl = (import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5100').replace(/\/$/, '')
const apiRoot = `${apiBaseUrl}/api/v1`

export type AdminSession = { id: string; name: string; email: string }
export type ArtistAdmin = { id: string; displayName: string; sortName?: string; biography?: string; location?: string; activeYears?: string; website?: string }
export type WorkAdmin = { id: string; title: string; creationDate?: string; medium?: string; description?: string; city?: string; context?: string; artists: { artistId: string; displayName: string }[] }
export type PublicationAdmin = { id: string; title: string; type: string; publisher?: string; publicationDate?: string; isbn?: string; issn?: string; description?: string }

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${apiRoot}${path}`, { ...init, credentials: 'include' })
  if (!response.ok) {
    const problem = await response.json().catch(() => null) as { detail?: string; title?: string } | null
    throw new Error(problem?.detail ?? problem?.title ?? `Request failed with status ${response.status}`)
  }
  if (response.status === 204) return undefined as T
  return response.json() as Promise<T>
}

async function mutation<T>(path: string, method: 'POST' | 'PUT' | 'DELETE', body?: unknown): Promise<T> {
  const { token } = await request<{ token: string }>('/auth/antiforgery')
  return request<T>(path, {
    method,
    headers: { 'Content-Type': 'application/json', 'X-XSRF-TOKEN': token },
    body: body === undefined ? undefined : JSON.stringify(body),
  })
}

export const adminService = {
  loginUrl: `${apiRoot}/auth/login/google`,
  session: () => request<AdminSession>('/auth/me'),
  logout: () => mutation<void>('/auth/logout', 'POST'),
  artists: () => request<ArtistAdmin[]>('/artists'),
  createArtist: (body: Omit<ArtistAdmin, 'id'>) => mutation<ArtistAdmin>('/artists', 'POST', body),
  updateArtist: (id: string, body: Omit<ArtistAdmin, 'id'>) => mutation<ArtistAdmin>(`/artists/${id}`, 'PUT', body),
  deleteArtist: (id: string) => mutation<void>(`/artists/${id}`, 'DELETE'),
  works: () => request<WorkAdmin[]>('/works'),
  createWork: (body: Record<string, unknown>) => mutation<WorkAdmin>('/works', 'POST', body),
  updateWork: (id: string, body: Record<string, unknown>) => mutation<WorkAdmin>(`/works/${id}`, 'PUT', body),
  deleteWork: (id: string) => mutation<void>(`/works/${id}`, 'DELETE'),
  publications: () => request<PublicationAdmin[]>('/publications'),
  createPublication: (body: Omit<PublicationAdmin, 'id'>) => mutation<PublicationAdmin>('/publications', 'POST', body),
  updatePublication: (id: string, body: Omit<PublicationAdmin, 'id'>) => mutation<PublicationAdmin>(`/publications/${id}`, 'PUT', body),
  deletePublication: (id: string) => mutation<void>(`/publications/${id}`, 'DELETE'),
}
