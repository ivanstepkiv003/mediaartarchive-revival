import { ArtistCard } from '../components/ArtistCard'
import { PageHeader } from '../components/PageHeader'
import { archiveService } from '../services/archiveService'
import { useTranslation } from 'react-i18next'

export function ArtistsPage() {
  const { t } = useTranslation()
  const artists = archiveService.getArtists()
  return <><PageHeader eyebrow={t('artists.eyebrow')} title={t('artists.title')} description={t('artists.description')} count={artists.length} /><div className="container artist-grid">{artists.map((artist) => <ArtistCard key={artist.id} artist={artist} />)}</div></>
}
