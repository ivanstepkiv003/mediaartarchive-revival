import type { ArchiveEvent } from '../models/types'
import { useTranslation } from 'react-i18next'

export function EventCard({ event }: { event: ArchiveEvent }) {
  const { t } = useTranslation()
  const statusLabel = event.status === 'Upcoming' ? t('events.upcoming') : t('events.past')
  return (
    <article className="event-card">
      <div><p className="event-date">{event.date}</p><span className={`status status-${event.status.toLowerCase()}`}>{statusLabel}</span></div>
      <div><h2>{event.title}</h2><p className="event-place">{event.venue} · {event.location}</p><p>{event.description}</p></div>
      <span className="event-arrow" aria-hidden="true">↗</span>
    </article>
  )
}
