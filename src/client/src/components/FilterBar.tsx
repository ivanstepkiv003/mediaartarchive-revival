import { useTranslation } from 'react-i18next'

export function FilterBar() {
  const { t } = useTranslation()
  const filters = ['artist', 'year', 'medium', 'city', 'context']
  return (
    <div className="filter-panel">
      <label className="search-field"><span className="sr-only">{t('archive.searchLabel')}</span><span aria-hidden="true">⌕</span><input type="search" placeholder={t('archive.search')} /></label>
      <div className="filter-row" aria-label={t('archive.filtersLabel')}>{filters.map((filter) => <button type="button" key={filter}>{t(`archive.filters.${filter}`)}<span aria-hidden="true">＋</span></button>)}</div>
    </div>
  )
}
