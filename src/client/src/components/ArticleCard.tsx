import { Link } from 'react-router-dom'
import type { Article } from '../models/types'

export function ArticleCard({ article, featured = false }: { article: Article; featured?: boolean }) {
  return (
    <article className={`article-card${featured ? ' article-featured' : ''}`}>
      <Link to={`/articles/${article.id}`}>
        <p className="eyebrow">{article.category}</p><h3>{article.title}</h3><p className="article-excerpt">{article.excerpt}</p>
        <div className="article-byline"><span>{article.author}</span><time>{article.publishedAt}</time></div>
      </Link>
    </article>
  )
}
