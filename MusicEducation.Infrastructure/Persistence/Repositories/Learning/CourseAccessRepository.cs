using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Learning;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Learning;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Learning;

public class CourseAccessRepository : ICourseAccessRepository
{
    private readonly MusicEducationDbContext _context;

    public CourseAccessRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<CourseAccess?> GetByIdAsync(int id)
    {
        return await _context.Set<CourseAccess>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<CourseAccess?> GetByUserIdAndCourseIdAsync(
        int userId,
        int courseId)
    {
        return await _context.Set<CourseAccess>()
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.CourseId == courseId);
    }

    public async Task<IEnumerable<CourseAccess>> GetByUserIdAsync(int userId)
    {
        return await _context.Set<CourseAccess>()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<CourseAccess>> GetByCourseIdAsync(int courseId)
    {
        return await _context.Set<CourseAccess>()
            .Where(x => x.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<bool> HasAccessAsync(int userId, int courseId)
    {
        return await _context.Set<CourseAccess>()
            .AnyAsync(x =>
                x.UserId == userId &&
                x.CourseId == courseId &&
                x.Status == CourseAccessStatus.Active);
    }

    public async Task<CourseAccess> AddAsync(CourseAccess courseAccess)
    {
        ArgumentNullException.ThrowIfNull(courseAccess);

        await _context.Set<CourseAccess>()
            .AddAsync(courseAccess);

        return courseAccess;
    }

    public Task UpdateAsync(CourseAccess courseAccess)
    {
        ArgumentNullException.ThrowIfNull(courseAccess);

        _context.Set<CourseAccess>()
            .Update(courseAccess);

        return Task.CompletedTask;
    }
}