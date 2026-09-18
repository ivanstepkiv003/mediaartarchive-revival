using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MediaArtArchive.Api.Persistence;

public sealed class ArchiveDbContextFactory : IDesignTimeDbContextFactory<ArchiveDbContext>
{
    public ArchiveDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ArchiveDbContext>()
            .UseNpgsql("Host=localhost;Database=archive;Username=postgres")
            .Options;

        return new ArchiveDbContext(options);
    }
}
