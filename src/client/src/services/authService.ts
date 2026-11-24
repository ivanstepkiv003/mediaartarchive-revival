const apiBaseUrl = (import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5100').replace(/\/$/, '')
export const apiRoot = `${apiBaseUrl}/api/v1`

export type UserSession = { id: string; name: string; email: string; isAdmin: boolean }

export async function apiRequest<T>(path: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${apiRoot}${path}`, { ...init, credentials: 'include' })
  if (!response.ok) {
    const problem = await response.json().catch(() => null) as { detail?: string; title?: string } | null
    throw new Error(problem?.detail ?? problem?.title ?? `Request failed with status ${response.status}`)
  }
  if (response.status === 204) return undefined as T
  return response.json() as Promise<T>
}

export async function authenticatedMutation<T>(path: string, method: 'POST' | 'PUT' | 'DELETE', body?: unknown): Promise<T> {
  const { token } = await apiRequest<{ token: string }>('/auth/antiforgery')
  return apiRequest<T>(path, {
    method,
    headers: { 'Content-Type': 'application/json', 'X-XSRF-TOKEN': token },
    body: body === undefined ? undefined : JSON.stringify(body),
  })
}

export const authService = {
  loginUrl: (returnUrl = '/') => `${apiRoot}/auth/login/google?returnUrl=${encodeURIComponent(returnUrl)}`,
  session: () => apiRequest<UserSession>('/auth/me'),
  logout: () => authenticatedMutation<void>('/auth/logout', 'POST'),
}
