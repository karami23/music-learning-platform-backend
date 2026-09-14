using MusicEducation.Application.Interfaces;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly MusicEducationDbContext _context;

    public UnitOfWork(MusicEducationDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
