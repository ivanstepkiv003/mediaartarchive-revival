import { useCallback, useEffect, useState, type Dispatch, type FormEvent, type SetStateAction } from 'react'
import { useTranslation } from 'react-i18next'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { adminService, type ArticleAdmin, type ArtistAdmin, type EventAdmin, type NewsAdmin, type PublicationAdmin, type WorkAdmin } from '../services/adminService'
import { authService, type UserSession } from '../services/authService'
import { archiveDataChangedEvent } from '../services/archiveData'

type Section = 'artists' | 'works' | 'publications' | 'articles' | 'events' | 'news'
const emptyArtist = { displayName: '', displayNameUk: '', sortName: '', sortNameUk: '', biography: '', biographyUk: '', location: '', locationUk: '', activeYears: '', activeYearsUk: '', website: '', imageUrl: '', tags: '' }
const emptyWork = { title: '', titleUk: '', creationDate: '', medium: '', mediumUk: '', description: '', descriptionUk: '', city: '', cityUk: '', context: '', contextUk: '', accent: '#9898a7', featured: false, imageUrl: '', tags: '', artistIds: [] as string[] }
const emptyPublication = { title: '', type: '', publisher: '', publicationDate: '', isbn: '', issn: '', description: '', tags: '' }
const emptyArticle = { title: '', titleUk: '', byline: '', bylineUk: '', publishedAt: '', category: '', categoryUk: '', excerpt: '', excerptUk: '', imageUrl: '', tags: '' }
const emptyEvent = { title: '', titleUk: '', eventDate: '', location: '', locationUk: '', venue: '', venueUk: '', status: 'Upcoming' as 'Upcoming' | 'Past', description: '', descriptionUk: '', imageUrl: '', tags: '' }
const emptyNews = { publishedAt: '', title: '', titleUk: '', summary: '', summaryUk: '', imageUrl: '', tags: '' }
const parseTags = (value: string) => value.split(',').map(tag => tag.trim()).filter(Boolean)
const formatTags = (tags?: string[]) => (tags ?? []).join(', ')

export function AdminPage() {
  const { t } = useTranslation()
  const navigate = useNavigate()
  const { artistId } = useParams()
  const [session, setSession] = useState<UserSession | null>()
  const [section, setSection] = useState<Section>('artists')
  const [artists, setArtists] = useState<ArtistAdmin[]>([])
  const [works, setWorks] = useState<WorkAdmin[]>([])
  const [publications, setPublications] = useState<PublicationAdmin[]>([])
  const [articles, setArticles] = useState<ArticleAdmin[]>([])
  const [events, setEvents] = useState<EventAdmin[]>([])
  const [news, setNews] = useState<NewsAdmin[]>([])
  const [artistForm, setArtistForm] = useState(emptyArtist)
  const [workForm, setWorkForm] = useState(emptyWork)
  const [publicationForm, setPublicationForm] = useState(emptyPublication)
  const [articleForm, setArticleForm] = useState(emptyArticle)
  const [eventForm, setEventForm] = useState(emptyEvent)
  const [newsForm, setNewsForm] = useState(emptyNews)
  const [editingId, setEditingId] = useState<string>()
  const [artistEditorOpen, setArtistEditorOpen] = useState(false)
  const [message, setMessage] = useState(() =>
    new URLSearchParams(window.location.search).has('error')
      ? t('admin.loginError')
      : '')

  const loadContent = useCallback(async () => {
    const [artistRecords, workRecords, publicationRecords, articleRecords, eventRecords, newsRecords] = await Promise.all([
      adminService.artists(), adminService.works(), adminService.publications(), adminService.articles(), adminService.events(), adminService.news(),
    ])
    setArtists(artistRecords)
    setWorks(workRecords)
    setPublications(publicationRecords)
    setArticles(articleRecords)
    setEvents(eventRecords)
    setNews(newsRecords)
  }, [])

  useEffect(() => {
    authService.session()
      .then(async current => {
        if (current.isAdmin) await loadContent()
        setSession(current)
      })
      .catch(() => setSession(null))
  }, [loadContent])

  useEffect(() => {
    if (!artistEditorOpen) return
    const previousOverflow = document.body.style.overflow
    const closeOnEscape = (event: KeyboardEvent) => {
      if (event.key === 'Escape') setArtistEditorOpen(false)
    }
    document.body.style.overflow = 'hidden'
    window.addEventListener('keydown', closeOnEscape)
    return () => {
      document.body.style.overflow = previousOverflow
      window.removeEventListener('keydown', closeOnEscape)
    }
  }, [artistEditorOpen])

  function resetEditor() {
    setArtistEditorOpen(false)
    setEditingId(undefined)
    setArtistForm(emptyArtist)
    setWorkForm(emptyWork)
    setPublicationForm(emptyPublication)
    setArticleForm(emptyArticle)
    setEventForm(emptyEvent)
    setNewsForm(emptyNews)
    setMessage('')
  }

  function openNewArtist() {
    setEditingId(undefined)
    setArtistForm(emptyArtist)
    setMessage('')
    setArtistEditorOpen(true)
  }

  async function runMutation(action: () => Promise<unknown>, success: string) {
    try {
      setMessage(t('admin.saving'))
      await action()
      await loadContent()
      window.dispatchEvent(new Event(archiveDataChangedEvent))
      resetEditor()
      setMessage(success)
      return true
    } catch (error) {
      setMessage(error instanceof Error ? error.message : t('admin.requestFailed'))
      return false
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
        <a className="admin-primary" href={authService.loginUrl(artistId ? window.location.pathname : '/admin')}>{t('admin.continueGoogle')}</a>
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
    void runMutation(() => adminService.createArtist({ ...artistForm, tags: parseTags(artistForm.tags) }), t('admin.created', { type: t('admin.artist') }))
  }
  const submitWork = (event: FormEvent) => {
    event.preventDefault()
    const payload = { ...workForm, tags: parseTags(workForm.tags) }
    const action = editingId ? adminService.updateWork(editingId, payload) : adminService.createWork(payload)
    void runMutation(() => action, t(editingId ? 'admin.updated' : 'admin.created', { type: t('admin.work') }))
  }
  const submitPublication = (event: FormEvent) => {
    event.preventDefault()
    const payload = { ...publicationForm, tags: parseTags(publicationForm.tags) }
    const action = editingId ? adminService.updatePublication(editingId, payload) : adminService.createPublication(payload)
    void runMutation(() => action, t(editingId ? 'admin.updated' : 'admin.created', { type: t('admin.publication') }))
  }
  const submitArticle = (event: FormEvent) => {
    event.preventDefault()
    const payload = { ...articleForm, tags: parseTags(articleForm.tags) }
    const action = editingId ? adminService.updateArticle(editingId, payload) : adminService.createArticle(payload)
    void runMutation(() => action, t(editingId ? 'admin.updated' : 'admin.created', { type: 'article' }))
  }
  const submitEvent = (event: FormEvent) => {
    event.preventDefault()
    const payload = { ...eventForm, tags: parseTags(eventForm.tags) }
    const action = editingId ? adminService.updateEvent(editingId, payload) : adminService.createEvent(payload)
    void runMutation(() => action, t(editingId ? 'admin.updated' : 'admin.created', { type: 'event' }))
  }
  const submitNews = (event: FormEvent) => {
    event.preventDefault()
    const payload = { ...newsForm, tags: parseTags(newsForm.tags) }
    const action = editingId ? adminService.updateNews(editingId, payload) : adminService.createNews(payload)
    void runMutation(() => action, t(editingId ? 'admin.updated' : 'admin.created', { type: 'news item' }))
  }

  if (artistId) {
    const artist = artists.find(record => record.id === artistId)
    if (!artist) return (
      <main className="admin-shell admin-edit-page">
        <Link className="admin-page-back" to="/admin">← {t('admin.backToArtists')}</Link>
        <div className="admin-edit-not-found"><p className="eyebrow">{t('admin.artist')}</p><h1>{t('admin.artistNotFound')}</h1></div>
      </main>
    )
    return <ArtistEditPage artist={artist} message={message} onSave={async form => {
      const saved = await runMutation(
        () => adminService.updateArtist(artist.id, { ...form, tags: parseTags(form.tags) }),
        t('admin.updated', { type: t('admin.artist') }),
      )
      if (saved) navigate('/admin')
    }} />
  }

  return (
    <main className="admin-shell">
      <header className="admin-header">
        <div><p className="eyebrow">{t('admin.contentManagement')}</p><h1>{t('admin.adminTitle')}</h1></div>
        <div className="admin-user"><span>{session.name || session.email}</span><button onClick={() => void authService.logout().then(() => setSession(null))}>{t('admin.signOut')}</button></div>
      </header>
      <nav className="admin-tabs" aria-label="Content types">
        {(['artists', 'works', 'articles', 'events', 'news', 'publications'] as Section[]).map(item =>
          <button className={section === item ? 'active' : ''} key={item} onClick={() => { setSection(item); resetEditor() }}>{t(`admin.${item}`)}</button>)}
      </nav>
      {message && <p className="admin-message" role="status">{message}</p>}

      <div className={`admin-workspace${section === 'artists' ? ' admin-workspace-list' : ''}`}>
        <section className="admin-records">
          <div className="admin-section-title"><h2>{t(`admin.${section}`, section)} <span>{{ artists: artists.length, works: works.length, publications: publications.length, articles: articles.length, events: events.length, news: news.length }[section]}</span></h2><button onClick={section === 'artists' ? openNewArtist : resetEditor}>{t(section === 'artists' ? 'admin.newArtist' : 'admin.newRecord')}</button></div>
          {section === 'artists' && artists.length === 0 && <div className="admin-empty"><p>{t('admin.noArtists')}</p></div>}
          {section === 'artists' && artists.map(artist => <RecordRow key={artist.id} title={artist.displayName} meta={artist.location} editLabel={t('admin.edit')} deleteLabel={t('admin.delete')} onEdit={() => navigate(`/admin/artists/${artist.id}/edit`)} onDelete={() => confirmDelete(t('admin.confirmDelete', { type: t('admin.artist'), title: artist.displayName }), () => adminService.deleteArtist(artist.id), runMutation, t('admin.deleted', { type: t('admin.artist') }))} />)}
          {section === 'works' && works.map(work => <RecordRow key={work.id} title={work.title} meta={work.artists.map(x => x.displayName).join(', ')} editLabel={t('admin.edit')} deleteLabel={t('admin.delete')} onEdit={() => { setEditingId(work.id); setWorkForm({ title: work.title, titleUk: work.titleUk ?? '', creationDate: work.creationDate ?? '', medium: work.medium ?? '', mediumUk: work.mediumUk ?? '', description: work.description ?? '', descriptionUk: work.descriptionUk ?? '', city: work.city ?? '', cityUk: work.cityUk ?? '', context: work.context ?? '', contextUk: work.contextUk ?? '', accent: work.accent ?? '#9898a7', featured: work.featured, imageUrl: work.imageUrl ?? '', tags: formatTags(work.tags), artistIds: work.artists.map(x => x.artistId) }) }} onDelete={() => confirmDelete(t('admin.confirmDelete', { type: t('admin.work'), title: work.title }), () => adminService.deleteWork(work.id), runMutation, t('admin.deleted', { type: t('admin.work') }))} />)}
          {section === 'publications' && publications.map(publication => <RecordRow key={publication.id} title={publication.title} meta={[publication.type, publication.publisher].filter(Boolean).join(' · ')} editLabel={t('admin.edit')} deleteLabel={t('admin.delete')} onEdit={() => { setEditingId(publication.id); setPublicationForm({ title: publication.title, type: publication.type, publisher: publication.publisher ?? '', publicationDate: publication.publicationDate ?? '', isbn: publication.isbn ?? '', issn: publication.issn ?? '', description: publication.description ?? '', tags: formatTags(publication.tags) }) }} onDelete={() => confirmDelete(t('admin.confirmDelete', { type: t('admin.publication'), title: publication.title }), () => adminService.deletePublication(publication.id), runMutation, t('admin.deleted', { type: t('admin.publication') }))} />)}
          {section === 'articles' && articles.map(article => <RecordRow key={article.id} title={article.title} meta={`${article.category} · ${article.publishedAt}`} editLabel={t('admin.edit')} deleteLabel={t('admin.delete')} onEdit={() => { setEditingId(article.id); setArticleForm({ title: article.title, titleUk: article.titleUk ?? '', byline: article.byline, bylineUk: article.bylineUk ?? '', publishedAt: article.publishedAt, category: article.category, categoryUk: article.categoryUk ?? '', excerpt: article.excerpt, excerptUk: article.excerptUk ?? '', imageUrl: article.imageUrl ?? '', tags: formatTags(article.tags) }) }} onDelete={() => confirmDelete(`Delete article “${article.title}”?`, () => adminService.deleteArticle(article.id), runMutation, 'Article deleted.')} />)}
          {section === 'events' && events.map(item => <RecordRow key={item.id} title={item.title} meta={`${item.eventDate} · ${item.location}`} editLabel={t('admin.edit')} deleteLabel={t('admin.delete')} onEdit={() => { setEditingId(item.id); setEventForm({ title: item.title, titleUk: item.titleUk ?? '', eventDate: item.eventDate, location: item.location, locationUk: item.locationUk ?? '', venue: item.venue, venueUk: item.venueUk ?? '', status: item.status, description: item.description, descriptionUk: item.descriptionUk ?? '', imageUrl: item.imageUrl ?? '', tags: formatTags(item.tags) }) }} onDelete={() => confirmDelete(`Delete event “${item.title}”?`, () => adminService.deleteEvent(item.id), runMutation, 'Event deleted.')} />)}
          {section === 'news' && news.map(item => <RecordRow key={item.id} title={item.title} meta={item.publishedAt} editLabel={t('admin.edit')} deleteLabel={t('admin.delete')} onEdit={() => { setEditingId(item.id); setNewsForm({ publishedAt: item.publishedAt, title: item.title, titleUk: item.titleUk ?? '', summary: item.summary, summaryUk: item.summaryUk ?? '', imageUrl: item.imageUrl ?? '', tags: formatTags(item.tags) }) }} onDelete={() => confirmDelete(`Delete news item “${item.title}”?`, () => adminService.deleteNews(item.id), runMutation, 'News item deleted.')} />)}
        </section>

        {section !== 'artists' && <section className="admin-editor">
          <p className="eyebrow">{t(editingId ? 'admin.editRecord' : 'admin.newRecord')}</p>
          {section === 'works' && <form onSubmit={submitWork}>
            <Field label="Title (English)" required value={workForm.title} onChange={title => setWorkForm({ ...workForm, title })} /><Field label="Title (Ukrainian)" value={workForm.titleUk} onChange={titleUk => setWorkForm({ ...workForm, titleUk })} />
            <Field label={t('admin.creationDate')} value={workForm.creationDate} onChange={creationDate => setWorkForm({ ...workForm, creationDate })} /><Field label="Medium (English)" value={workForm.medium} onChange={medium => setWorkForm({ ...workForm, medium })} /><Field label="Medium (Ukrainian)" value={workForm.mediumUk} onChange={mediumUk => setWorkForm({ ...workForm, mediumUk })} />
            <Field label="City (English)" value={workForm.city} onChange={city => setWorkForm({ ...workForm, city })} /><Field label="City (Ukrainian)" value={workForm.cityUk} onChange={cityUk => setWorkForm({ ...workForm, cityUk })} /><Field label="Context (English)" value={workForm.context} onChange={context => setWorkForm({ ...workForm, context })} /><Field label="Context (Ukrainian)" value={workForm.contextUk} onChange={contextUk => setWorkForm({ ...workForm, contextUk })} />
            <Field label="Description (English)" multiline value={workForm.description} onChange={description => setWorkForm({ ...workForm, description })} /><Field label="Description (Ukrainian)" multiline value={workForm.descriptionUk} onChange={descriptionUk => setWorkForm({ ...workForm, descriptionUk })} /><Field label="Accent color" type="color" value={workForm.accent} onChange={accent => setWorkForm({ ...workForm, accent })} />
            <label className="admin-check"><input type="checkbox" checked={workForm.featured} onChange={event => setWorkForm({ ...workForm, featured: event.target.checked })} />Featured on home page</label>
            <Field label={t('admin.imageUrl')} type="url" value={workForm.imageUrl} onChange={imageUrl => setWorkForm({ ...workForm, imageUrl })} />
            <Field label={t('admin.tags')} value={workForm.tags} onChange={tags => setWorkForm({ ...workForm, tags })} />
            <fieldset><legend>{t('admin.workArtists')}</legend>{artists.map(artist => <label className="admin-check" key={artist.id}><input type="checkbox" checked={workForm.artistIds.includes(artist.id)} onChange={() => setWorkForm({ ...workForm, artistIds: workForm.artistIds.includes(artist.id) ? workForm.artistIds.filter(id => id !== artist.id) : [...workForm.artistIds, artist.id] })} />{artist.displayName}</label>)}</fieldset><Submit editing={!!editingId} createLabel={t('admin.createRecord')} saveLabel={t('admin.saveChanges')} />
          </form>}
          {section === 'publications' && <form onSubmit={submitPublication}><Field label={t('admin.publicationTitle')} required value={publicationForm.title} onChange={title => setPublicationForm({ ...publicationForm, title })} /><Field label={t('admin.type')} required value={publicationForm.type} onChange={type => setPublicationForm({ ...publicationForm, type })} /><Field label={t('admin.publisher')} value={publicationForm.publisher} onChange={publisher => setPublicationForm({ ...publicationForm, publisher })} /><Field label={t('admin.publicationDate')} type="date" value={publicationForm.publicationDate} onChange={publicationDate => setPublicationForm({ ...publicationForm, publicationDate })} /><Field label={t('admin.isbn')} value={publicationForm.isbn} onChange={isbn => setPublicationForm({ ...publicationForm, isbn })} /><Field label={t('admin.issn')} value={publicationForm.issn} onChange={issn => setPublicationForm({ ...publicationForm, issn })} /><Field label={t('admin.description')} multiline value={publicationForm.description} onChange={description => setPublicationForm({ ...publicationForm, description })} /><Field label={t('admin.tags')} value={publicationForm.tags} onChange={tags => setPublicationForm({ ...publicationForm, tags })} /><Submit editing={!!editingId} createLabel={t('admin.createRecord')} saveLabel={t('admin.saveChanges')} /></form>}
          {section === 'articles' && <form onSubmit={submitArticle}><LocalizedFields form={articleForm} setForm={setArticleForm} fields={[['title', 'Title'], ['byline', 'Author'], ['category', 'Category'], ['excerpt', 'Excerpt', true]]} /><Field label="Publication date" type="date" required value={articleForm.publishedAt} onChange={publishedAt => setArticleForm({ ...articleForm, publishedAt })} />
            <Field label={t('admin.imageUrl')} type="url" value={articleForm.imageUrl ?? ''} onChange={imageUrl => setArticleForm({ ...articleForm, imageUrl })} />
            <Field label={t('admin.tags')} value={articleForm.tags} onChange={tags => setArticleForm({ ...articleForm, tags })} />
            <Submit editing={!!editingId} createLabel={t('admin.createRecord')} saveLabel={t('admin.saveChanges')} />
          </form>}
          {section === 'events' && <form onSubmit={submitEvent}><LocalizedFields form={eventForm} setForm={setEventForm} fields={[['title', 'Title'], ['location', 'Location'], ['venue', 'Venue'], ['description', 'Description', true]]} /><Field label="Event date" type="date" required value={eventForm.eventDate} onChange={eventDate => setEventForm({ ...eventForm, eventDate })} /><label className="admin-field"><span>Status</span><select value={eventForm.status} onChange={event => setEventForm({ ...eventForm, status: event.target.value as 'Upcoming' | 'Past' })}><option>Upcoming</option><option>Past</option></select></label>
            <Field label={t('admin.imageUrl')} type="url" value={eventForm.imageUrl ?? ''} onChange={imageUrl => setEventForm({ ...eventForm, imageUrl })} />
            <Field label={t('admin.tags')} value={eventForm.tags} onChange={tags => setEventForm({ ...eventForm, tags })} />
            <Submit editing={!!editingId} createLabel={t('admin.createRecord')} saveLabel={t('admin.saveChanges')} />
          </form>}
          {section === 'news' && <form onSubmit={submitNews}><LocalizedFields form={newsForm} setForm={setNewsForm} fields={[['title', 'Title'], ['summary', 'Summary', true]]} /><Field label="Publication date" type="date" required value={newsForm.publishedAt} onChange={publishedAt => setNewsForm({ ...newsForm, publishedAt })} />
            <Field label={t('admin.imageUrl')} type="url" value={newsForm.imageUrl ?? ''} onChange={imageUrl => setNewsForm({ ...newsForm, imageUrl })} />
            <Field label={t('admin.tags')} value={newsForm.tags} onChange={tags => setNewsForm({ ...newsForm, tags })} />
            <Submit editing={!!editingId} createLabel={t('admin.createRecord')} saveLabel={t('admin.saveChanges')} />
          </form>}
        </section>}
      </div>

      {artistEditorOpen && <div className="admin-modal-backdrop" onMouseDown={event => { if (event.target === event.currentTarget) resetEditor() }}>
        <section className="admin-modal" role="dialog" aria-modal="true" aria-labelledby="artist-editor-title">
          <header><div><p className="eyebrow">{t('admin.artist')}</p><h2 id="artist-editor-title">{t('admin.newArtist')}</h2></div><button type="button" aria-label={t('admin.closeEditor')} onClick={resetEditor}>×</button></header>
          {message && <p className="admin-message" role="status">{message}</p>}
          <form onSubmit={submitArtist}>
            <Field label="Display name (English)" required value={artistForm.displayName} onChange={displayName => setArtistForm({ ...artistForm, displayName })} />
            <Field label="Display name (Ukrainian)" value={artistForm.displayNameUk} onChange={displayNameUk => setArtistForm({ ...artistForm, displayNameUk })} />
            <Field label="Sort name (English)" value={artistForm.sortName} onChange={sortName => setArtistForm({ ...artistForm, sortName })} />
            <Field label="Sort name (Ukrainian)" value={artistForm.sortNameUk} onChange={sortNameUk => setArtistForm({ ...artistForm, sortNameUk })} />
            <Field label="Location (English)" value={artistForm.location} onChange={location => setArtistForm({ ...artistForm, location })} />
            <Field label="Location (Ukrainian)" value={artistForm.locationUk} onChange={locationUk => setArtistForm({ ...artistForm, locationUk })} />
            <Field label="Active years (English)" value={artistForm.activeYears} onChange={activeYears => setArtistForm({ ...artistForm, activeYears })} />
            <Field label="Active years (Ukrainian)" value={artistForm.activeYearsUk} onChange={activeYearsUk => setArtistForm({ ...artistForm, activeYearsUk })} />
            <Field label={t('admin.website')} type="url" value={artistForm.website} onChange={website => setArtistForm({ ...artistForm, website })} />
            <Field label={t('admin.imageUrl')} type="url" value={artistForm.imageUrl} onChange={imageUrl => setArtistForm({ ...artistForm, imageUrl })} />
            <Field label={t('admin.tags')} value={artistForm.tags} onChange={tags => setArtistForm({ ...artistForm, tags })} />
            <Field label="Biography (English)" multiline value={artistForm.biography} onChange={biography => setArtistForm({ ...artistForm, biography })} />
            <Field label="Biography (Ukrainian)" multiline value={artistForm.biographyUk} onChange={biographyUk => setArtistForm({ ...artistForm, biographyUk })} />
            <div className="admin-modal-actions"><button type="button" onClick={resetEditor}>{t('admin.cancel')}</button><Submit editing={false} createLabel={t('admin.createRecord')} saveLabel={t('admin.saveChanges')} /></div>
          </form>
        </section>
      </div>}
    </main>
  )
}

function ArtistEditPage({ artist, message, onSave }: { artist: ArtistAdmin; message: string; onSave: (form: typeof emptyArtist) => Promise<void> }) {
  const { t } = useTranslation()
  const [form, setForm] = useState(() => ({
    displayName: artist.displayName,
    displayNameUk: artist.displayNameUk ?? '',
    sortName: artist.sortName ?? '',
    sortNameUk: artist.sortNameUk ?? '',
    biography: artist.biography ?? '',
    biographyUk: artist.biographyUk ?? '',
    location: artist.location ?? '',
    locationUk: artist.locationUk ?? '',
    activeYears: artist.activeYears ?? '',
    activeYearsUk: artist.activeYearsUk ?? '',
    website: artist.website ?? '',
    imageUrl: artist.imageUrl ?? '',
    tags: formatTags(artist.tags),
  }))
  const submit = (event: FormEvent) => {
    event.preventDefault()
    void onSave(form)
  }

  return <main className="admin-shell admin-edit-page">
    <header className="admin-edit-header"><div><Link className="admin-page-back" to="/admin">← {t('admin.backToArtists')}</Link><p className="eyebrow">{t('admin.artist')}</p><h1>{t('admin.editArtist')}</h1></div><span>{artist.displayName}</span></header>
    {message && <p className="admin-message" role="status">{message}</p>}
    <section className="admin-editor admin-editor-page">
      <form onSubmit={submit}>
        <Field label="Display name (English)" required value={form.displayName} onChange={displayName => setForm({ ...form, displayName })} />
        <Field label="Display name (Ukrainian)" value={form.displayNameUk} onChange={displayNameUk => setForm({ ...form, displayNameUk })} />
        <Field label="Sort name (English)" value={form.sortName} onChange={sortName => setForm({ ...form, sortName })} />
        <Field label="Sort name (Ukrainian)" value={form.sortNameUk} onChange={sortNameUk => setForm({ ...form, sortNameUk })} />
        <Field label="Location (English)" value={form.location} onChange={location => setForm({ ...form, location })} />
        <Field label="Location (Ukrainian)" value={form.locationUk} onChange={locationUk => setForm({ ...form, locationUk })} />
        <Field label="Active years (English)" value={form.activeYears} onChange={activeYears => setForm({ ...form, activeYears })} />
        <Field label="Active years (Ukrainian)" value={form.activeYearsUk} onChange={activeYearsUk => setForm({ ...form, activeYearsUk })} />
        <Field label={t('admin.website')} type="url" value={form.website} onChange={website => setForm({ ...form, website })} />
        <Field label={t('admin.imageUrl')} type="url" value={form.imageUrl} onChange={imageUrl => setForm({ ...form, imageUrl })} />
        <Field label={t('admin.tags')} value={form.tags} onChange={tags => setForm({ ...form, tags })} />
        <Field label="Biography (English)" multiline value={form.biography} onChange={biography => setForm({ ...form, biography })} />
        <Field label="Biography (Ukrainian)" multiline value={form.biographyUk} onChange={biographyUk => setForm({ ...form, biographyUk })} />
        <div className="admin-modal-actions"><Link to="/admin">{t('admin.cancel')}</Link><Submit editing createLabel={t('admin.createRecord')} saveLabel={t('admin.saveChanges')} /></div>
      </form>
    </section>
  </main>
}

function Field({ label, value, onChange, required = false, multiline = false, type = 'text' }: { label: string; value: string; onChange: (value: string) => void; required?: boolean; multiline?: boolean; type?: string }) {
  return <label className="admin-field"><span>{label}</span>{multiline ? <textarea required={required} value={value} onChange={event => onChange(event.target.value)} /> : <input type={type} required={required} value={value} onChange={event => onChange(event.target.value)} />}</label>
}

function LocalizedFields<T extends Record<string, string>>({ form, setForm, fields }: { form: T; setForm: Dispatch<SetStateAction<T>>; fields: [keyof T & string, string, boolean?][] }) {
  return <>{fields.map(([key, label, multiline]) => {
    const ukKey = `${key}Uk` as keyof T & string
    return <div key={key}><Field label={`${label} (English)`} required value={form[key]} multiline={multiline} onChange={value => setForm({ ...form, [key]: value })} /><Field label={`${label} (Ukrainian)`} value={form[ukKey]} multiline={multiline} onChange={value => setForm({ ...form, [ukKey]: value })} /></div>
  })}</>
}

function Submit({ editing, createLabel, saveLabel }: { editing: boolean; createLabel: string; saveLabel: string }) { return <button className="admin-primary" type="submit">{editing ? saveLabel : createLabel}</button> }

function RecordRow({ title, meta, editLabel, deleteLabel, onEdit, onDelete }: { title: string; meta?: string; editLabel: string; deleteLabel: string; onEdit: () => void; onDelete: () => void }) {
  return <article className="admin-record"><div><h3>{title}</h3>{meta && <p>{meta}</p>}</div><div><button onClick={onEdit}>{editLabel}</button><button className="danger" onClick={onDelete}>{deleteLabel}</button></div></article>
}

function confirmDelete(prompt: string, action: () => Promise<unknown>, run: (action: () => Promise<unknown>, success: string) => Promise<unknown>, success: string) {
  if (window.confirm(prompt)) void run(action, success)
}
