import { useTranslation } from 'react-i18next'

export function ArchiveDataState({ loading, error }: { loading: boolean; error: string | null }) {
  const { t } = useTranslation()
  return <main className="container data-state" role={error ? 'alert' : 'status'}>{error ? t('common.loadError') : loading ? t('common.loading') : null}</main>
}
