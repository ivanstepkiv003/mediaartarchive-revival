import { Link } from 'react-router-dom'
import { ArticleCard } from '../components/ArticleCard'
import { ArtworkCard } from '../components/ArtworkCard'
import { EventCard } from '../components/EventCard'
import { SectionHeader } from '../components/SectionHeader'
import { useArchiveData } from '../services/archiveData'
import { ArchiveDataState } from '../components/ArchiveDataState'
import { useTranslation } from 'react-i18next'

export function HomePage() {
  const { t } = useTranslation()
  const { artworks, artists, articles, events, news, loading, error } = useArchiveData()
  if (loading || error) return <ArchiveDataState loading={loading} error={error} />

  return <>
    <section className="home-hero container">
      <div className="hero-index"><span>{t('home.index')}</span><span>{t('home.established')}</span></div>
      <h1>{t('home.title1')}<br /><em>{t('home.title2')}</em><br />{t('home.title3')}</h1>
      <div className="hero-intro"><p>{t('home.intro')}</p><Link className="circle-link" to="/archive" aria-label={t('home.explore')}><span>{t('home.explore')}</span><span aria-hidden="true">↘</span></Link></div>
    </section>

    <section className="home-section container">
      <SectionHeader kicker={t('home.selected')} title={t('home.featured')} link="/archive" />
      <div className="featured-grid">{artworks.filter((artwork) => artwork.featured).map((artwork, index) => <ArtworkCard key={artwork.id} artwork={artwork} large={index === 0} />)}</div>
    </section>

    <section className="explore-section">
      <div className="container"><SectionHeader kicker={t('home.browse')} title={t('home.exploreTitle')} /></div>
      <div className="explore-links container">
        <Link to="/archive"><span className="explore-number">01</span><span>{t('home.artworks')}</span><span className="explore-meta">{t('common.records', { count: artworks.length })}</span><span aria-hidden="true">→</span></Link>
        <Link to="/artists"><span className="explore-number">02</span><span>{t('home.artists')}</span><span className="explore-meta">{t('common.records', { count: artists.length })}</span><span aria-hidden="true">→</span></Link>
      </div>
    </section>

    <section className="home-section container">
      <SectionHeader kicker={t('home.writing')} title={t('home.latestArticles')} link="/articles" />
      <div className="article-grid">{articles[0] && <ArticleCard article={articles[0]} featured />}{articles.slice(1).map((article) => <ArticleCard key={article.id} article={article} />)}</div>
    </section>

    <section className="split-section container">
      <div><SectionHeader title={t('home.news')} link="/news" />{news.slice(0, 2).map((item) => <article className="news-compact" key={item.id}><time>{item.date}</time><h3><Link to="/news">{item.title}</Link></h3></article>)}</div>
      <div><SectionHeader title={t('home.events')} link="/events" />{events.slice(0, 1).map((event) => <EventCard key={event.id} event={event} />)}</div>
    </section>
  </>
}
