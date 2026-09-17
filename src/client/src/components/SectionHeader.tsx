import { Link } from 'react-router-dom'
import { useTranslation } from 'react-i18next'

type SectionHeaderProps = { title: string; link?: string; linkLabel?: string; kicker?: string }

export function SectionHeader({ title, link, linkLabel, kicker }: SectionHeaderProps) {
  const { t } = useTranslation()
  return (
    <div className="section-header">
      <div>{kicker && <p className="eyebrow">{kicker}</p>}<h2>{title}</h2></div>
      {link && <Link className="text-link" to={link}>{linkLabel ?? t('common.viewAll')}<span aria-hidden="true">↗</span></Link>}
    </div>
  )
}
