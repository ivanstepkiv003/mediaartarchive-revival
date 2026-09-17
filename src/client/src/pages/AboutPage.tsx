import { PageHeader } from '../components/PageHeader'
import { useTranslation } from 'react-i18next'

export function AboutPage() {
  const { t } = useTranslation()
  return <article><PageHeader eyebrow={t('about.eyebrow')} title={t('about.title')} description={t('about.description')} /><div className="about-intro container"><p>{t('about.intro')}</p><span className="about-symbol" aria-hidden="true">У</span></div><div className="about-sections container"><section><span>01</span><div><h2>{t('about.purpose')}</h2><p>{t('about.purposeText')}</p></div></section><section><span>02</span><div><h2>{t('about.history')}</h2><p>{t('about.historyText')}</p></div></section><section><span>03</span><div><h2>{t('about.revival')}</h2><p>{t('about.revivalText')}</p></div></section><section><span>04</span><div><h2>{t('about.credits')}</h2><p>{t('about.creditsText')}</p></div></section></div><div className="placeholder-notice container"><strong>{t('about.prototype')}</strong><p>{t('about.prototypeText')}</p></div></article>
}
