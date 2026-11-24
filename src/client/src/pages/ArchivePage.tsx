import { ArtworkCard } from '../components/ArtworkCard'
import { FilterBar } from '../components/FilterBar'
import { PageHeader } from '../components/PageHeader'
import { useArchiveData } from '../services/archiveData'
import { ArchiveDataState } from '../components/ArchiveDataState'
import { useTranslation } from 'react-i18next'

export function ArchivePage() {
  const { t } = useTranslation()
  const { artworks, loading, error } = useArchiveData()
  if (loading || error) return <ArchiveDataState loading={loading} error={error} />
  return <>
    <PageHeader eyebrow={t('archive.eyebrow')} title={t('archive.title')} description={t('archive.description')} count={artworks.length} />
    <div className="container"><FilterBar /><div className="results-line"><span>{t('common.allArtworks')}</span><span>{t('common.sorted')}</span></div><div className="archive-grid">{artworks.map((artwork) => <ArtworkCard key={artwork.id} artwork={artwork} />)}</div></div>
  </>
}
