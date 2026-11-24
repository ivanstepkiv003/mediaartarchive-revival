import { useEffect, useState } from 'react'
import { Link, NavLink, useLocation } from 'react-router-dom'
import { useTranslation } from 'react-i18next'
import { siteConfig } from '../config/site'
import { authService, type UserSession } from '../services/authService'

const navigation = [
  ['nav.archive', '/archive'], ['nav.artists', '/artists'], ['nav.articles', '/articles'],
  ['nav.news', '/news'], ['nav.events', '/events'], ['nav.about', '/about'],
]

export function Header() {
  const [open, setOpen] = useState(false)
  const [session, setSession] = useState<UserSession | null>()
  const location = useLocation()
  const { t, i18n } = useTranslation()
  const language = i18n.resolvedLanguage?.startsWith('uk') ? 'uk' : 'en'
  const authError = new URLSearchParams(location.search).has('authError')

  const changeLanguage = (nextLanguage: 'uk' | 'en') => void i18n.changeLanguage(nextLanguage)

  useEffect(() => {
    authService.session().then(setSession).catch(() => setSession(null))
  }, [])

  const account = session === undefined ? null : session === null
    ? <a className="account-action" href={authService.loginUrl(`${location.pathname}${location.search}`)}>{t('auth.signIn')}</a>
    : <div className="account-session">
        <span title={session.email}>{session.name || session.email}</span>
        {session.isAdmin && <Link to="/admin">{t('auth.admin')}</Link>}
        <button type="button" onClick={() => void authService.logout().then(() => setSession(null))}>{t('auth.signOut')}</button>
      </div>

  const nav = (
    <nav className="site-nav" aria-label={t('nav.mainLabel')}>
      {navigation.map(([label, path]) => (
        <NavLink key={path} to={path} onClick={() => setOpen(false)} className={({ isActive }) => isActive ? 'active' : ''}>{t(label)}</NavLink>
      ))}
    </nav>
  )

  return (
    <header className="site-header">
      <div className="header-inner">
        <Link className="wordmark" to="/" aria-label={t('common.home')} onClick={() => setOpen(false)}>
          <span className="wordmark-mark" aria-hidden="true">{siteConfig.mark}</span>
          <span>{t('platform.label')}</span>
        </Link>
        <div className="header-actions"><div className="desktop-nav">{nav}</div>{authError && <span className="auth-error" role="alert">{t('auth.loginError')}</span>}{account}<div className="language-switcher" aria-label={t('common.language')}><button className={language === 'uk' ? 'active' : ''} aria-pressed={language === 'uk'} onClick={() => changeLanguage('uk')} type="button">UA</button><span>/</span><button className={language === 'en' ? 'active' : ''} aria-pressed={language === 'en'} onClick={() => changeLanguage('en')} type="button">EN</button></div></div>
        <button className="menu-button" type="button" aria-expanded={open} aria-controls="mobile-menu" onClick={() => setOpen(!open)}>
          <span>{open ? t('nav.close') : t('nav.menu')}</span><span aria-hidden="true">{open ? '×' : '≡'}</span>
        </button>
      </div>
      <div className="archive-sentence">
        <p>{t('platform.statement')} <Link to={siteConfig.programmePath}>{t('nav.collection')}</Link>, <Link to="/articles">{t('nav.research')}</Link>, <Link to="/about">{t('nav.preservation')}</Link>.</p>
      </div>
      {open && <div id="mobile-menu" className="mobile-nav" key={location.pathname}>{nav}</div>}
    </header>
  )
}
