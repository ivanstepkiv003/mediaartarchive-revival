import { useParams } from 'react-router-dom'
import { ArtworkCard } from '../components/ArtworkCard'
import { MetadataList } from '../components/MetadataList'
import { SectionHeader } from '../components/SectionHeader'
import { useArchiveData } from '../services/archiveData'
import { ArchiveDataState } from '../components/ArchiveDataState'
import { NotFoundPage } from './NotFoundPage'
import { useTranslation } from 'react-i18next'
import { TagList } from '../components/TagList'

export function ArtistDetailPage() {
  const { t } = useTranslation()
  const { artistId } = useParams()
  const { artists, artworks, articles, events, loading, error } = useArchiveData()
  if (loading || error) return <ArchiveDataState loading={loading} error={error} />
  const artist = artists.find((item) => item.id === artistId)
  if (!artist) return <NotFoundPage />
  const works = artworks.filter((work) => work.artistId === artist.id)
  return <article>
    <header className="artist-detail-hero container">
      <div className="artist-portrait artist-portrait-large" aria-hidden="true">
        {artist.imageUrl ? <img src={artist.imageUrl} alt="" referrerPolicy="no-referrer" /> : artist.initials}
      </div>
      <div><p className="eyebrow">{t('artists.record')}</p><h1>{artist.name}</h1><p className="detail-lead">{artist.bio}</p><TagList tags={artist.tags} /></div>
    </header>
    <div className="detail-body container"><div><p className="eyebrow">{t('artists.biography')}</p><p>{artist.bio} {t('artists.biographyExtra')}</p></div><MetadataList items={[[t('artists.location'), artist.location], [t('artists.period'), artist.activeYears], [t('common.recordStatus'), t('artists.placeholderBio')]]} /></div>
    <section className="home-section container"><SectionHeader title={t('artists.selected')} link="/archive" />{works.length ? <div className="archive-grid">{works.map((work) => <ArtworkCard key={work.id} artwork={work} />)}</div> : <p>{t('artists.noWorks')}</p>}</section>
    {(articles[0] || events[0]) && <section className="related-context container"><SectionHeader title={t('artists.context')} />{articles[0] && <div><span>{t('artists.essay')}</span><p>{articles[0].title}</p><span>2025</span></div>}{events[0] && <div><span>{t('artists.event')}</span><p>{events[0].title}</p><span>2026</span></div>}</section>}
  </article>
}
