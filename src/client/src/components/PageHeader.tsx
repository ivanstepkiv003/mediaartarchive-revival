import { useTranslation } from 'react-i18next'

type PageHeaderProps = { eyebrow?: string; title: string; description?: string; count?: number }

export function PageHeader({ eyebrow, title, description, count }: PageHeaderProps) {
  const { t } = useTranslation()
  return (
    <header className="page-header container">
      <div>{eyebrow && <p className="eyebrow">{eyebrow}</p>}<h1>{title}</h1></div>
      {(description || count !== undefined) && <div className="page-intro">{description && <p>{description}</p>}{count !== undefined && <p className="record-count">{t('common.records', { count: count.toString().padStart(2, '0') })}</p>}</div>}
    </header>
  )
}
