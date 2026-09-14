using Microsoft.EntityFrameworkCore;

namespace MusicEducation.Infrastructure.Persistence.DbContext;

public class MusicEducationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public MusicEducationDbContext(DbContextOptions<MusicEducationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MusicEducationDbContext).Assembly);
    }
}