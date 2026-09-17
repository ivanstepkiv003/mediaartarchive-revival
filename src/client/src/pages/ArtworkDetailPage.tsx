import { Link, useParams } from 'react-router-dom'
import { ArtworkCard } from '../components/ArtworkCard'
import { MetadataList } from '../components/MetadataList'
import { SectionHeader } from '../components/SectionHeader'
import { archiveService } from '../services/archiveService'
import { NotFoundPage } from './NotFoundPage'
import { useTranslation } from 'react-i18next'

export function ArtworkDetailPage() {
  const { t } = useTranslation()
  const { artworkId } = useParams()
  const artwork = archiveService.getArtwork(artworkId ?? '')
  if (!artwork) return <NotFoundPage />
  const related = archiveService.getArtworks().filter((item) => item.id !== artwork.id).slice(0, 3)
  return <article>
    <header className="detail-header container"><p className="eyebrow">{t('artwork.record')}</p><div className="detail-heading"><h1>{artwork.title}</h1><p><Link to={`/artists/${artwork.artistId}`}>{artwork.artist}</Link><br />{artwork.year}</p></div></header>
    <div className="artwork-media-full" style={{ '--art-accent': artwork.accent } as React.CSSProperties} role="img" aria-label={`${t('artwork.mediaPlaceholder')}: ${artwork.title}`}><span>{t('artwork.mediaPlaceholder')}</span><strong aria-hidden="true">{artwork.title.slice(0, 2)}</strong></div>
    <div className="detail-body container"><div><p className="eyebrow">{t('artwork.about')}</p><p className="detail-lead">{artwork.description}</p><p>{t('artwork.body')}</p></div><MetadataList items={[[t('artwork.artist'), artwork.artist], [t('artwork.year'), artwork.year], [t('artwork.media'), artwork.medium], [t('artwork.location'), artwork.city], [t('artwork.context'), artwork.context], [t('common.recordStatus'), t('common.placeholder')]]} /></div>
    <section className="resources container"><SectionHeader title={t('artwork.documentation')} /><div className="resource-row"><span>{t('artwork.liveVariant')}</span><span>{t('artwork.liveStatus')}</span><span aria-hidden="true">↗</span></div><div className="resource-row"><span>{t('artwork.archiveVariant')}</span><span>{t('artwork.archiveStatus')}</span><span aria-hidden="true">→</span></div><div className="resource-row"><span>{t('artwork.installation')}</span><span>{t('artwork.pdf')}</span><span aria-hidden="true">↓</span></div></section>
    <section className="home-section container"><SectionHeader title={t('artwork.related')} link="/archive" /><div className="archive-grid">{related.map((item) => <ArtworkCard key={item.id} artwork={item} />)}</div></section>
  </article>
}
