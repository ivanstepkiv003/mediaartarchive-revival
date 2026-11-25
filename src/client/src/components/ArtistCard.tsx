import { Link } from 'react-router-dom'
import type { Artist } from '../models/types'
import { TagList } from './TagList'

export function ArtistCard({ artist }: { artist: Artist }) {
  return (
    <article className="artist-card">
      <Link to={`/artists/${artist.id}`}>
        <div className="artist-portrait" aria-hidden="true">
          {artist.imageUrl ? <img src={artist.imageUrl} alt="" loading="lazy" referrerPolicy="no-referrer" /> : artist.initials}
        </div>
        <div className="card-heading"><h2>{artist.name}</h2><span aria-hidden="true">↗</span></div>
        <p>{artist.location}</p><p className="muted">{artist.activeYears}</p><p className="artist-summary">{artist.bio}</p><TagList tags={artist.tags} compact />
      </Link>
    </article>
  )
}
