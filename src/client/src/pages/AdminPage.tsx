import { useCallback, useEffect, useState, type FormEvent } from 'react'
import { useTranslation } from 'react-i18next'
import { adminService, type ArtistAdmin, type PublicationAdmin, type WorkAdmin } from '../services/adminService'
import { authService, type UserSession } from '../services/authService'

type Section = 'artists' | 'works' | 'publications'
const emptyArtist = { displayName: '', sortName: '', biography: '', location: '', activeYears: '', website: '' }
const emptyWork = { title: '', creationDate: '', medium: '', description: '', city: '', context: '', artistIds: [] as string[] }
const emptyPublication = { title: '', type: '', publisher: '', publicationDate: '', isbn: '', issn: '', description: '' }

export function AdminPage() {
  const { t } = useTranslation()
  const [session, setSession] = useState<UserSession | null>()
  const [section, setSection] = useState<Section>('artists')
  const [artists, setArtists] = useState<ArtistAdmin[]>([])
  const [works, setWorks] = useState<WorkAdmin[]>([])
  const [publications, setPublications] = useState<PublicationAdmin[]>([])
  const [artistForm, setArtistForm] = useState(emptyArtist)
  const [workForm, setWorkForm] = useState(emptyWork)
  const [publicationForm, setPublicationForm] = useState(emptyPublication)
  const [editingId, setEditingId] = useState<string>()
  const [message, setMessage] = useState(() =>
    new URLSearchParams(window.location.search).has('error')
      ? t('admin.loginError')
      : '')

  const loadContent = useCallback(async () => {
    const [artistRecords, workRecords, publicationRecords] = await Promise.all([
      adminService.artists(), adminService.works(), adminService.publications(),
    ])
    setArtists(artistRecords)
    setWorks(workRecords)
    setPublications(publicationRecords)
  }, [])

  useEffect(() => {
    authService.session()
      .then(async current => {
        setSession(current)
        if (current.isAdmin) await loadContent()
      })
      .catch(() => setSession(null))
  }, [loadContent])

  function resetEditor() {
    setEditingId(undefined)
    setArtistForm(emptyArtist)
    setWorkForm(emptyWork)
    setPublicationForm(emptyPublication)
    setMessage('')
  }

  async function runMutation(action: () => Promise<unknown>, success: string) {
    try {
      setMessage(t('admin.saving'))
      await action()
      await loadContent()
      resetEditor()
      setMessage(success)
    } catch (error) {
      setMessage(error instanceof Error ? error.message : t('admin.requestFailed'))
    }
  }

  if (session === undefined) return <main className="admin-login"><p>{t('admin.loading')}</p></main>
  if (session === null) return (
    <main className="admin-login">
      <div>
        <p className="eyebrow">{t('admin.restricted')}</p>
        <h1>{t('admin.title')}</h1>
        <p>{t('admin.loginPrompt')}</p>
        {message && <p className="admin-message" role="alert">{message}</p>}
        <a className="admin-primary" href={authService.loginUrl('/admin')}>{t('admin.continueGoogle')}</a>
        <a className="admin-back" href="/">{t('admin.returnArchive')}</a>
      </div>
    </main>
  )
  if (!session.isAdmin) return (
    <main className="admin-login">
      <div>
        <p className="eyebrow">{t('admin.restricted')}</p>
        <h1>{t('auth.notAuthorizedTitle')}</h1>
        <p>{t('auth.notAuthorized')}</p>
        <button className="admin-primary" onClick={() => void authService.logout().then(() => setSession(null))}>{t('admin.signOut')}</button>
        <a className="admin-back" href="/">{t('admin.returnArchive')}</a>
      </div>
    </main>
  )

  const submitArtist = (event: FormEvent) => {
    event.preventDefault()
    const action = editingId ? adminService.updateArtist(editingId, artistForm) : adminService.createArtist(artistForm)
    void runMutation(() => action, t(editingId ? 'admin.updated' : 'admin.created', { type: t('admin.artist') }))
  }
  const submitWork = (event: FormEvent) => {
    event.preventDefault()
    const action = editingId ? adminService.updateWork(editingId, workForm) : adminService.createWork(workForm)
    void runMutation(() => action, t(editingId ? 'admin.updated' : 'admin.created', { type: t('admin.work') }))
  }
  const submitPublication = (event: FormEvent) => {
    event.preventDefault()
    const action = editingId ? adminService.updatePublication(editingId, publicationForm) : adminService.createPublication(publicationForm)
    void runMutation(() => action, t(editingId ? 'admin.updated' : 'admin.created', { type: t('admin.publication') }))
  }

  return (
    <main className="admin-shell">
      <header className="admin-header">
        <div><p className="eyebrow">{t('admin.contentManagement')}</p><h1>{t('admin.adminTitle')}</h1></div>
        <div className="admin-user"><span>{session.name || session.email}</span><button onClick={() => void authService.logout().then(() => setSession(null))}>{t('admin.signOut')}</button></div>
      </header>
      <nav className="admin-tabs" aria-label="Content types">
        {(['artists', 'works', 'publications'] as Section[]).map(item =>
          <button className={section === item ? 'active' : ''} key={item} onClick={() => { setSection(item); resetEditor() }}>{t(`admin.${item}`)}</button>)}
      </nav>
      {message && <p className="admin-message" role="status">{message}</p>}

      <div className="admin-workspace">
        <section className="admin-records">
          <div className="admin-section-title"><h2>{t(`admin.${section}`)}</h2><button onClick={resetEditor}>{t('admin.newRecord')}</button></div>
          {section === 'artists' && artists.map(artist => <RecordRow key={artist.id} title={artist.displayName} meta={artist.location} editLabel={t('admin.edit')} deleteLabel={t('admin.delete')} onEdit={() => { setEditingId(artist.id); setArtistForm({ displayName: artist.displayName, sortName: artist.sortName ?? '', biography: artist.biography ?? '', location: artist.location ?? '', activeYears: artist.activeYears ?? '', website: artist.website ?? '' }) }} onDelete={() => confirmDelete(t('admin.confirmDelete', { type: t('admin.artist'), title: artist.displayName }), () => adminService.deleteArtist(artist.id), runMutation, t('admin.deleted', { type: t('admin.artist') }))} />)}
          {section === 'works' && works.map(work => <RecordRow key={work.id} title={work.title} meta={work.artists.map(x => x.displayName).join(', ')} editLabel={t('admin.edit')} deleteLabel={t('admin.delete')} onEdit={() => { setEditingId(work.id); setWorkForm({ title: work.title, creationDate: work.creationDate ?? '', medium: work.medium ?? '', description: work.description ?? '', city: work.city ?? '', context: work.context ?? '', artistIds: work.artists.map(x => x.artistId) }) }} onDelete={() => confirmDelete(t('admin.confirmDelete', { type: t('admin.work'), title: work.title }), () => adminService.deleteWork(work.id), runMutation, t('admin.deleted', { type: t('admin.work') }))} />)}
          {section === 'publications' && publications.map(publication => <RecordRow key={publication.id} title={publication.title} meta={[publication.type, publication.publisher].filter(Boolean).join(' · ')} editLabel={t('admin.edit')} deleteLabel={t('admin.delete')} onEdit={() => { setEditingId(publication.id); setPublicationForm({ title: publication.title, type: publication.type, publisher: publication.publisher ?? '', publicationDate: publication.publicationDate ?? '', isbn: publication.isbn ?? '', issn: publication.issn ?? '', description: publication.description ?? '' }) }} onDelete={() => confirmDelete(t('admin.confirmDelete', { type: t('admin.publication'), title: publication.title }), () => adminService.deletePublication(publication.id), runMutation, t('admin.deleted', { type: t('admin.publication') }))} />)}
        </section>

        <section className="admin-editor">
          <p className="eyebrow">{t(editingId ? 'admin.editRecord' : 'admin.newRecord')}</p>
          {section === 'artists' && <form onSubmit={submitArtist}><Field label={t('admin.displayName')} required value={artistForm.displayName} onChange={displayName => setArtistForm({ ...artistForm, displayName })} /><Field label={t('admin.sortName')} value={artistForm.sortName} onChange={sortName => setArtistForm({ ...artistForm, sortName })} /><Field label={t('admin.location')} value={artistForm.location} onChange={location => setArtistForm({ ...artistForm, location })} /><Field label={t('admin.activeYears')} value={artistForm.activeYears} onChange={activeYears => setArtistForm({ ...artistForm, activeYears })} /><Field label={t('admin.website')} type="url" value={artistForm.website} onChange={website => setArtistForm({ ...artistForm, website })} /><Field label={t('admin.biography')} multiline value={artistForm.biography} onChange={biography => setArtistForm({ ...artistForm, biography })} /><Submit editing={!!editingId} createLabel={t('admin.createRecord')} saveLabel={t('admin.saveChanges')} /></form>}
          {section === 'works' && <form onSubmit={submitWork}><Field label={t('admin.workTitle')} required value={workForm.title} onChange={title => setWorkForm({ ...workForm, title })} /><Field label={t('admin.creationDate')} value={workForm.creationDate} onChange={creationDate => setWorkForm({ ...workForm, creationDate })} /><Field label={t('admin.medium')} value={workForm.medium} onChange={medium => setWorkForm({ ...workForm, medium })} /><Field label={t('admin.city')} value={workForm.city} onChange={city => setWorkForm({ ...workForm, city })} /><Field label={t('admin.context')} value={workForm.context} onChange={context => setWorkForm({ ...workForm, context })} /><Field label={t('admin.description')} multiline value={workForm.description} onChange={description => setWorkForm({ ...workForm, description })} /><fieldset><legend>{t('admin.workArtists')}</legend>{artists.map(artist => <label className="admin-check" key={artist.id}><input type="checkbox" checked={workForm.artistIds.includes(artist.id)} onChange={() => setWorkForm({ ...workForm, artistIds: workForm.artistIds.includes(artist.id) ? workForm.artistIds.filter(id => id !== artist.id) : [...workForm.artistIds, artist.id] })} />{artist.displayName}</label>)}</fieldset><Submit editing={!!editingId} createLabel={t('admin.createRecord')} saveLabel={t('admin.saveChanges')} /></form>}
          {section === 'publications' && <form onSubmit={submitPublication}><Field label={t('admin.publicationTitle')} required value={publicationForm.title} onChange={title => setPublicationForm({ ...publicationForm, title })} /><Field label={t('admin.type')} required value={publicationForm.type} onChange={type => setPublicationForm({ ...publicationForm, type })} /><Field label={t('admin.publisher')} value={publicationForm.publisher} onChange={publisher => setPublicationForm({ ...publicationForm, publisher })} /><Field label={t('admin.publicationDate')} type="date" value={publicationForm.publicationDate} onChange={publicationDate => setPublicationForm({ ...publicationForm, publicationDate })} /><Field label={t('admin.isbn')} value={publicationForm.isbn} onChange={isbn => setPublicationForm({ ...publicationForm, isbn })} /><Field label={t('admin.issn')} value={publicationForm.issn} onChange={issn => setPublicationForm({ ...publicationForm, issn })} /><Field label={t('admin.description')} multiline value={publicationForm.description} onChange={description => setPublicationForm({ ...publicationForm, description })} /><Submit editing={!!editingId} createLabel={t('admin.createRecord')} saveLabel={t('admin.saveChanges')} /></form>}
        </section>
      </div>
    </main>
  )
}

function Field({ label, value, onChange, required = false, multiline = false, type = 'text' }: { label: string; value: string; onChange: (value: string) => void; required?: boolean; multiline?: boolean; type?: string }) {
  return <label className="admin-field"><span>{label}</span>{multiline ? <textarea required={required} value={value} onChange={event => onChange(event.target.value)} /> : <input type={type} required={required} value={value} onChange={event => onChange(event.target.value)} />}</label>
}

function Submit({ editing, createLabel, saveLabel }: { editing: boolean; createLabel: string; saveLabel: string }) { return <button className="admin-primary" type="submit">{editing ? saveLabel : createLabel}</button> }

function RecordRow({ title, meta, editLabel, deleteLabel, onEdit, onDelete }: { title: string; meta?: string; editLabel: string; deleteLabel: string; onEdit: () => void; onDelete: () => void }) {
  return <article className="admin-record"><div><h3>{title}</h3>{meta && <p>{meta}</p>}</div><div><button onClick={onEdit}>{editLabel}</button><button className="danger" onClick={onDelete}>{deleteLabel}</button></div></article>
}

function confirmDelete(prompt: string, action: () => Promise<unknown>, run: (action: () => Promise<unknown>, success: string) => Promise<void>, success: string) {
  if (window.confirm(prompt)) void run(action, success)
}
