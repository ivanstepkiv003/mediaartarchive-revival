import { PageHeader } from '../components/PageHeader'
import { archiveService } from '../services/archiveService'
import { useTranslation } from 'react-i18next'

export function NewsPage() {
  const { t } = useTranslation()
  const news = archiveService.getNews()
  return <><PageHeader eyebrow={t('news.eyebrow')} title={t('news.title')} description={t('news.description')} count={news.length} /><div className="container news-list">{news.map((item) => <article key={item.id}><time>{item.date}</time><div><h2>{item.title}</h2><p>{item.summary}</p></div><span aria-hidden="true">↗</span></article>)}</div></>
}
