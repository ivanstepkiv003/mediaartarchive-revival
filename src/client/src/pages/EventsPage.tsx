import { EventCard } from '../components/EventCard'
import { PageHeader } from '../components/PageHeader'
import { useArchiveData } from '../services/archiveData'
import { ArchiveDataState } from '../components/ArchiveDataState'
import { useTranslation } from 'react-i18next'

export function EventsPage() {
  const { t } = useTranslation()
  const { events, loading, error } = useArchiveData()
  if (loading || error) return <ArchiveDataState loading={loading} error={error} />
  return <><PageHeader eyebrow={t('events.eyebrow')} title={t('events.title')} description={t('events.description')} count={events.length} /><div className="container event-list">{events.map((event) => <EventCard key={event.id} event={event} />)}</div></>
}
