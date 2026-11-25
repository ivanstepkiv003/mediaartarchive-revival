import { PageHeader } from '../components/PageHeader'
import { useArchiveData } from '../services/archiveData'
import { ArchiveDataState } from '../components/ArchiveDataState'
import { useTranslation } from 'react-i18next'
import { TagList } from '../components/TagList'

export function NewsPage() {
  const { t } = useTranslation()
  const { news, loading, error } = useArchiveData()
  if (loading || error) return <ArchiveDataState loading={loading} error={error} />
  return <>
    <PageHeader eyebrow={t('news.eyebrow')} title={t('news.title')} description={t('news.description')} count={news.length} />
    <div className="container news-list">
      {news.map((item) => <article key={item.id}>
        <time>{item.date}</time>
        <div><h2>{item.title}</h2><p>{item.summary}</p><TagList tags={item.tags} compact /></div>
        {item.imageUrl && <img className="record-thumb" src={item.imageUrl} alt="" loading="lazy" referrerPolicy="no-referrer" />}
      </article>)}
    </div>
  </>
}
