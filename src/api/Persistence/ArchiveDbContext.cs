using Microsoft.EntityFrameworkCore;

namespace MediaArtArchive.Api.Persistence;

public sealed class ArchiveDbContext(DbContextOptions<ArchiveDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("archive");
        base.OnModelCreating(modelBuilder);
    }
}
