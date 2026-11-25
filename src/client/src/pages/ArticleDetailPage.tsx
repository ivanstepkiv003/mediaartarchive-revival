import { useParams } from 'react-router-dom'
import { useArchiveData } from '../services/archiveData'
import { ArchiveDataState } from '../components/ArchiveDataState'
import { NotFoundPage } from './NotFoundPage'
import { useTranslation } from 'react-i18next'
import { TagList } from '../components/TagList'

export function ArticleDetailPage() {
  const { t } = useTranslation()
  const { articleId } = useParams()
  const { articles, loading, error } = useArchiveData()
  if (loading || error) return <ArchiveDataState loading={loading} error={error} />
  const article = articles.find((item) => item.id === articleId)
  if (!article) return <NotFoundPage />
  return <article className="longform container">
    <header><p className="eyebrow">{article.category}</p><h1>{article.title}</h1><p className="longform-deck">{article.excerpt}</p><TagList tags={article.tags} /><div className="article-byline"><span>{t('articles.by', { author: article.author })}</span><time>{article.publishedAt}</time><span>{t('articles.readTime')}</span></div></header>
    {article.imageUrl
      ? <img className="record-image" src={article.imageUrl} alt={article.title} referrerPolicy="no-referrer" />
      : <div className="article-visual" role="img" aria-label={t('articles.visual')}><span>{t('common.imagePlaceholder')}</span></div>}
    <div className="longform-body"><aside>{t('articles.notice')}</aside><div><p className="drop-cap">{t('articles.p1')}</p><h2>{t('articles.heading1')}</h2><p>{t('articles.p2')}</p><p>{t('articles.p3')}</p><blockquote>{t('articles.quote')}</blockquote><h2>{t('articles.heading2')}</h2><p>{t('articles.p4')}</p></div></div>
  </article>
}
