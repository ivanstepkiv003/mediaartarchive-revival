import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import App from './App'
import './i18n'
import './styles.css'
import { ArchiveDataProvider } from './services/archiveData'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <ArchiveDataProvider><App /></ArchiveDataProvider>
    </BrowserRouter>
  </StrictMode>,
)
