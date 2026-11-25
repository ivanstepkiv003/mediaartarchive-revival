import { useTranslation } from 'react-i18next'
import { Link } from 'react-router-dom'
import type { CSSProperties } from 'react'
import type { LatestArchiveItem } from '../services/archiveData'
import { TagList } from './TagList'

export function LatestGrid({ items }: { items: LatestArchiveItem[] }) {
  const { t } = useTranslation()
  if (!items.length) return null

  return <section className="latest-section container">
    <h1 className="sr-only">{t('latest.title')}</h1>
    <div className="latest-grid">
      {items.map((item) => <article className="latest-card" key={`${item.kind}-${item.id}`}>
        <Link to={item.href}>
          <div className="latest-media">
            {item.imageUrl
              ? <img src={item.imageUrl} alt="" loading="lazy" referrerPolicy="no-referrer" />
              : <div className={`latest-illustration latest-illustration-${item.kind}`} style={{ '--latest-accent': item.accent } as CSSProperties} aria-hidden="true"><span>{item.title.slice(0, 2)}</span></div>}
          </div>
          <div className="latest-information">
            <p className="eyebrow">{t(`latest.types.${item.kind}`)}</p>
            <h2>{item.title}</h2>
            <p>{item.summary}</p>
            <TagList tags={item.tags} compact />
            <span className="latest-meta">{item.meta}</span>
          </div>
        </Link>
      </article>)}
    </div>
  </section>
}
