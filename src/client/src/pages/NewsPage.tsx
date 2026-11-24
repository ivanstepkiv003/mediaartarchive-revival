import { PageHeader } from '../components/PageHeader'
import { useArchiveData } from '../services/archiveData'
import { ArchiveDataState } from '../components/ArchiveDataState'
import { useTranslation } from 'react-i18next'

export function NewsPage() {
  const { t } = useTranslation()
  const { news, loading, error } = useArchiveData()
  if (loading || error) return <ArchiveDataState loading={loading} error={error} />
  return <><PageHeader eyebrow={t('news.eyebrow')} title={t('news.title')} description={t('news.description')} count={news.length} /><div className="container news-list">{news.map((item) => <article key={item.id}><time>{item.date}</time><div><h2>{item.title}</h2><p>{item.summary}</p></div><span aria-hidden="true">↗</span></article>)}</div></>
}
