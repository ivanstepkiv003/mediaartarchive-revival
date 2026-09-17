import { Link } from 'react-router-dom'
import type { Artwork } from '../models/types'
import { useTranslation } from 'react-i18next'

export function ArtworkCard({ artwork, large = false }: { artwork: Artwork; large?: boolean }) {
  const { t } = useTranslation()
  return (
    <article className={`artwork-card${large ? ' artwork-card-large' : ''}`}>
      <Link to={`/archive/${artwork.id}`} aria-label={t('common.artworkBy', { title: artwork.title, artist: artwork.artist })}>
        <div className="artwork-image" style={{ '--art-accent': artwork.accent } as React.CSSProperties} role="img" aria-label={t('common.abstractPlaceholder', { title: artwork.title })}>
          <span className="placeholder-label">{t('common.imagePlaceholder')}</span><span className="artwork-glyph" aria-hidden="true">{artwork.title.slice(0, 2)}</span>
        </div>
        <div className="card-heading"><h3>{artwork.title}</h3><span>{artwork.year}</span></div>
        <p>{artwork.artist}</p><p className="muted">{artwork.medium}</p>
      </Link>
    </article>
  )
}
