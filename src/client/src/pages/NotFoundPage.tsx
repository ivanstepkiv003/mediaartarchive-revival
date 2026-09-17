import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'

export function NotFoundPage() {
  const { t } = useTranslation()
  return <div className="not-found container"><p className="eyebrow">{t('notFound.eyebrow')}</p><h1>{t('notFound.title')}</h1><Link className="text-link" to="/">{t('notFound.back')} <span aria-hidden="true">→</span></Link></div>
}
