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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("archive");

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.ToTable("artists");
            entity.HasKey(artist => artist.Id);
            entity.Property(artist => artist.DisplayName).HasMaxLength(240).IsRequired();
            entity.Property(artist => artist.SortName).HasMaxLength(240);
            entity.Property(artist => artist.Biography).HasMaxLength(10_000);
            entity.Property(artist => artist.Location).HasMaxLength(240);
            entity.Property(artist => artist.ActiveYears).HasMaxLength(120);
            entity.Property(artist => artist.Website).HasMaxLength(2_000);
            entity.HasIndex(artist => artist.SortName);
        });

        modelBuilder.Entity<Work>(entity =>
        {
            entity.ToTable("works");
            entity.HasKey(work => work.Id);
            entity.Property(work => work.Title).HasMaxLength(500).IsRequired();
            entity.Property(work => work.CreationDate).HasMaxLength(120);
            entity.Property(work => work.Medium).HasMaxLength(500);
            entity.Property(work => work.Description).HasMaxLength(20_000);
            entity.Property(work => work.City).HasMaxLength(240);
            entity.Property(work => work.Context).HasMaxLength(2_000);
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

        base.OnModelCreating(modelBuilder);
    }
}
