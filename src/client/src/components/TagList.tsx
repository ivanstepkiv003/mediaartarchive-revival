import { useTranslation } from 'react-i18next'

export function TagList({ tags = [], compact = false }: { tags?: string[]; compact?: boolean }) {
  const { t } = useTranslation()
  if (tags.length === 0) return null

  return <ul className={`tag-list${compact ? ' tag-list-compact' : ''}`} aria-label={t('common.tags')}>
    {tags.map(tag => <li key={tag}>#{tag}</li>)}
  </ul>
}
