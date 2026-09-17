import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'

export function Footer() {
  const { t } = useTranslation()
  return (
    <footer className="site-footer">
      <div className="footer-main">
        <p className="footer-title">{t('platform.shortLabel')}</p>
        <p className="footer-note">{t('footer.note')}</p>
        <nav aria-label={t('nav.footerLabel')}>
          <Link to="/about">{t('footer.about')}</Link><Link to="/articles">{t('footer.research')}</Link><Link to="/events">{t('footer.programme')}</Link>
        </nav>
      </div>
      <div className="footer-meta"><span>{t('platform.development')}</span><span>{t('common.placeholderContent')}</span></div>
    </footer>
  )
}
