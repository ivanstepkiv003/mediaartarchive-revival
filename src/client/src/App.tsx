import { Route, Routes } from 'react-router-dom'
import { AppLayout } from './components/AppLayout'
import { AboutPage } from './pages/AboutPage'
import { AdminPage } from './pages/AdminPage'
import { ArchivePage } from './pages/ArchivePage'
import { ArticleDetailPage } from './pages/ArticleDetailPage'
import { ArticlesPage } from './pages/ArticlesPage'
import { ArtistDetailPage } from './pages/ArtistDetailPage'
import { ArtistsPage } from './pages/ArtistsPage'
import { ArtworkDetailPage } from './pages/ArtworkDetailPage'
import { EventsPage } from './pages/EventsPage'
import { HomePage } from './pages/HomePage'
import { NewsPage } from './pages/NewsPage'
import { NotFoundPage } from './pages/NotFoundPage'

export default function App() {
  return (
    <Routes>
      <Route path="admin" element={<AdminPage />} />
      <Route element={<AppLayout />}>
        <Route index element={<HomePage />} />
        <Route path="archive" element={<ArchivePage />} />
        <Route path="archive/:artworkId" element={<ArtworkDetailPage />} />
        <Route path="artists" element={<ArtistsPage />} />
        <Route path="artists/:artistId" element={<ArtistDetailPage />} />
        <Route path="articles" element={<ArticlesPage />} />
        <Route path="articles/:articleId" element={<ArticleDetailPage />} />
        <Route path="news" element={<NewsPage />} />
        <Route path="events" element={<EventsPage />} />
        <Route path="about" element={<AboutPage />} />
        <Route path="*" element={<NotFoundPage />} />
      </Route>
    </Routes>
  )
}
