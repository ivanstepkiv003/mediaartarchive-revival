import { ArticleCard } from '../components/ArticleCard'
import { PageHeader } from '../components/PageHeader'
import { archiveService } from '../services/archiveService'
import { useTranslation } from 'react-i18next'

export function ArticlesPage() {
  const { t } = useTranslation()
  const articles = archiveService.getArticles()
  return <><PageHeader eyebrow={t('articles.eyebrow')} title={t('articles.title')} description={t('articles.description')} count={articles.length} /><div className="container article-list">{articles.map((article, index) => <ArticleCard key={article.id} article={article} featured={index === 0} />)}</div></>
}
