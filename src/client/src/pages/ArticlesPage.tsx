import { ArticleCard } from '../components/ArticleCard'
import { PageHeader } from '../components/PageHeader'
import { useArchiveData } from '../services/archiveData'
import { ArchiveDataState } from '../components/ArchiveDataState'
import { useTranslation } from 'react-i18next'

export function ArticlesPage() {
  const { t } = useTranslation()
  const { articles, loading, error } = useArchiveData()
  if (loading || error) return <ArchiveDataState loading={loading} error={error} />
  return <><PageHeader eyebrow={t('articles.eyebrow')} title={t('articles.title')} description={t('articles.description')} count={articles.length} /><div className="container article-list">{articles.map((article, index) => <ArticleCard key={article.id} article={article} featured={index === 0} />)}</div></>
}
