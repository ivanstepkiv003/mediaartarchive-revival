import { Link } from 'react-router-dom'
import type { Artist } from '../models/types'
import { useTranslation } from 'react-i18next'

export function ArtistCard({ artist }: { artist: Artist }) {
  const { t } = useTranslation()
  return (
    <article className="artist-card">
      <Link to={`/artists/${artist.id}`}>
        <div className="artist-portrait" role="img" aria-label={t('artists.portrait', { name: artist.name })}>{artist.initials}</div>
        <div className="card-heading"><h2>{artist.name}</h2><span aria-hidden="true">↗</span></div>
        <p>{artist.location}</p><p className="muted">{artist.activeYears}</p><p className="artist-summary">{artist.bio}</p>
      </Link>
    </article>
  )
}
