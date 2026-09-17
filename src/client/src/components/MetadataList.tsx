export function MetadataList({ items }: { items: Array<[string, string]> }) {
  return <dl className="metadata-list">{items.map(([term, value]) => <div key={term}><dt>{term}</dt><dd>{value}</dd></div>)}</dl>
}
