import { Outlet } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { Footer } from './Footer'
import { Header } from './Header'

export function AppLayout() {
  const { t } = useTranslation()
  return (
    <div className="app-shell">
      <a className="skip-link" href="#main-content">{t('common.skip')}</a>
      <Header />
      <main id="main-content"><Outlet /></main>
      <Footer />
    </div>
  )
}
