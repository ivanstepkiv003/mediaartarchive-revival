import { useParams } from 'react-router-dom'
import { ArtworkCard } from '../components/ArtworkCard'
import { MetadataList } from '../components/MetadataList'
import { SectionHeader } from '../components/SectionHeader'
import { archiveService } from '../services/archiveService'
import { NotFoundPage } from './NotFoundPage'
import { useTranslation } from 'react-i18next'

export function ArtistDetailPage() {
  const { t } = useTranslation()
  const { artistId } = useParams()
  const artist = archiveService.getArtist(artistId ?? '')
  if (!artist) return <NotFoundPage />
  const works = archiveService.getArtworks().filter((work) => work.artistId === artist.id)
  return <article>
    <header className="artist-detail-hero container"><div className="artist-portrait artist-portrait-large" role="img" aria-label={t('artists.portrait', { name: artist.name })}>{artist.initials}</div><div><p className="eyebrow">{t('artists.record')}</p><h1>{artist.name}</h1><p className="detail-lead">{artist.bio}</p></div></header>
    <div className="detail-body container"><div><p className="eyebrow">{t('artists.biography')}</p><p>{artist.bio} {t('artists.biographyExtra')}</p></div><MetadataList items={[[t('artists.location'), artist.location], [t('artists.period'), artist.activeYears], [t('common.recordStatus'), t('artists.placeholderBio')]]} /></div>
    <section className="home-section container"><SectionHeader title={t('artists.selected')} link="/archive" />{works.length ? <div className="archive-grid">{works.map((work) => <ArtworkCard key={work.id} artwork={work} />)}</div> : <p>{t('artists.noWorks')}</p>}</section>
    <section className="related-context container"><SectionHeader title={t('artists.context')} /><div><span>{t('artists.essay')}</span><p>{archiveService.getArticles()[0].title}</p><span>2025</span></div><div><span>{t('artists.event')}</span><p>{archiveService.getEvents()[0].title}</p><span>2026</span></div></section>
  </article>
}
