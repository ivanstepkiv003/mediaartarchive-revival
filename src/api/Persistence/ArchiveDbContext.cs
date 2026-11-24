using Microsoft.EntityFrameworkCore;
using MediaArtArchive.Api.Domain;

namespace MediaArtArchive.Api.Persistence;

public sealed class ArchiveDbContext(DbContextOptions<ArchiveDbContext> options)
    : DbContext(options)
{
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Work> Works => Set<Work>();
    public DbSet<WorkArtist> WorkArtists => Set<WorkArtist>();
    public DbSet<Publication> Publications => Set<Publication>();
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<ArchiveEvent> Events => Set<ArchiveEvent>();
    public DbSet<NewsItem> NewsItems => Set<NewsItem>();
    public DbSet<DataSeed> DataSeeds => Set<DataSeed>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("archive");

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.ToTable("artists");
            entity.HasKey(artist => artist.Id);
            entity.Property(artist => artist.DisplayName).HasMaxLength(240).IsRequired();
            entity.Property(artist => artist.DisplayNameUk).HasMaxLength(240);
            entity.Property(artist => artist.SortName).HasMaxLength(240);
            entity.Property(artist => artist.SortNameUk).HasMaxLength(240);
            entity.Property(artist => artist.Biography).HasMaxLength(10_000);
            entity.Property(artist => artist.BiographyUk).HasMaxLength(10_000);
            entity.Property(artist => artist.Location).HasMaxLength(240);
            entity.Property(artist => artist.LocationUk).HasMaxLength(240);
            entity.Property(artist => artist.ActiveYears).HasMaxLength(120);
            entity.Property(artist => artist.ActiveYearsUk).HasMaxLength(120);
            entity.Property(artist => artist.Website).HasMaxLength(2_000);
            entity.HasIndex(artist => artist.SortName);
        });

        modelBuilder.Entity<Work>(entity =>
        {
            entity.ToTable("works");
            entity.HasKey(work => work.Id);
            entity.Property(work => work.Title).HasMaxLength(500).IsRequired();
            entity.Property(work => work.TitleUk).HasMaxLength(500);
            entity.Property(work => work.CreationDate).HasMaxLength(120);
            entity.Property(work => work.Medium).HasMaxLength(500);
            entity.Property(work => work.MediumUk).HasMaxLength(500);
            entity.Property(work => work.Description).HasMaxLength(20_000);
            entity.Property(work => work.DescriptionUk).HasMaxLength(20_000);
            entity.Property(work => work.City).HasMaxLength(240);
            entity.Property(work => work.CityUk).HasMaxLength(240);
            entity.Property(work => work.Context).HasMaxLength(2_000);
            entity.Property(work => work.ContextUk).HasMaxLength(2_000);
            entity.Property(work => work.Accent).HasMaxLength(20);
            entity.HasIndex(work => work.Title);
        });

        modelBuilder.Entity<WorkArtist>(entity =>
        {
            entity.ToTable("work_artists");
            entity.HasKey(link => new { link.WorkId, link.ArtistId });
            entity.Property(link => link.Role).HasMaxLength(120);
            entity.HasOne(link => link.Work).WithMany(work => work.Artists)
                .HasForeignKey(link => link.WorkId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(link => link.Artist).WithMany(artist => artist.Works)
                .HasForeignKey(link => link.ArtistId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Publication>(entity =>
        {
            entity.ToTable("publications");
            entity.HasKey(publication => publication.Id);
            entity.Property(publication => publication.Title).HasMaxLength(500).IsRequired();
            entity.Property(publication => publication.Type).HasMaxLength(120).IsRequired();
            entity.Property(publication => publication.Publisher).HasMaxLength(240);
            entity.Property(publication => publication.Isbn).HasMaxLength(32);
            entity.Property(publication => publication.Issn).HasMaxLength(32);
            entity.Property(publication => publication.Description).HasMaxLength(20_000);
            entity.HasIndex(publication => publication.Title);
            entity.HasIndex(publication => publication.Isbn).IsUnique();
            entity.HasIndex(publication => publication.Issn).IsUnique();
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable("articles");
            entity.HasKey(article => article.Id);
            entity.Property(article => article.Title).HasMaxLength(500).IsRequired();
            entity.Property(article => article.TitleUk).HasMaxLength(500);
            entity.Property(article => article.Byline).HasMaxLength(240).IsRequired();
            entity.Property(article => article.BylineUk).HasMaxLength(240);
            entity.Property(article => article.Category).HasMaxLength(120).IsRequired();
            entity.Property(article => article.CategoryUk).HasMaxLength(120);
            entity.Property(article => article.Excerpt).HasMaxLength(5_000).IsRequired();
            entity.Property(article => article.ExcerptUk).HasMaxLength(5_000);
            entity.HasIndex(article => article.PublishedAt);
        });

        modelBuilder.Entity<ArchiveEvent>(entity =>
        {
            entity.ToTable("events");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Title).HasMaxLength(500).IsRequired();
            entity.Property(item => item.TitleUk).HasMaxLength(500);
            entity.Property(item => item.Location).HasMaxLength(240).IsRequired();
            entity.Property(item => item.LocationUk).HasMaxLength(240);
            entity.Property(item => item.Venue).HasMaxLength(500).IsRequired();
            entity.Property(item => item.VenueUk).HasMaxLength(500);
            entity.Property(item => item.Status).HasMaxLength(40).IsRequired();
            entity.Property(item => item.Description).HasMaxLength(5_000).IsRequired();
            entity.Property(item => item.DescriptionUk).HasMaxLength(5_000);
            entity.HasIndex(item => item.EventDate);
        });

        modelBuilder.Entity<NewsItem>(entity =>
        {
            entity.ToTable("news_items");
            entity.HasKey(item => item.Id);
            entity.Property(item => item.Title).HasMaxLength(500).IsRequired();
            entity.Property(item => item.TitleUk).HasMaxLength(500);
            entity.Property(item => item.Summary).HasMaxLength(5_000).IsRequired();
            entity.Property(item => item.SummaryUk).HasMaxLength(5_000);
            entity.HasIndex(item => item.PublishedAt);
        });

        modelBuilder.Entity<DataSeed>(entity =>
        {
            entity.ToTable("data_seeds");
            entity.HasKey(seed => seed.Name);
            entity.Property(seed => seed.Name).HasMaxLength(120);
        });

        base.OnModelCreating(modelBuilder);
    }
}
