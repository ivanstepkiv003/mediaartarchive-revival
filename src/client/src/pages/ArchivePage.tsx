import { ArtworkCard } from '../components/ArtworkCard'
import { FilterBar } from '../components/FilterBar'
import { PageHeader } from '../components/PageHeader'
import { archiveService } from '../services/archiveService'
import { useTranslation } from 'react-i18next'

export function ArchivePage() {
  const { t } = useTranslation()
  const artworks = archiveService.getArtworks()
  return <>
    <PageHeader eyebrow={t('archive.eyebrow')} title={t('archive.title')} description={t('archive.description')} count={artworks.length} />
    <div className="container"><FilterBar /><div className="results-line"><span>{t('common.allArtworks')}</span><span>{t('common.sorted')}</span></div><div className="archive-grid">{artworks.map((artwork) => <ArtworkCard key={artwork.id} artwork={artwork} />)}</div></div>
  </>
}
