using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Interfaces.Courses;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Courses;

public class ChapterRepository : IChapterRepository
{
    private readonly MusicEducationDbContext _context;

    public ChapterRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<Chapter?> GetByIdAsync(int id)
    {
        return await _context.Set<Chapter>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Chapter>> GetAllAsync()
    {
        return await _context.Set<Chapter>()
            .ToListAsync();
    }

    public async Task<IEnumerable<Chapter>> GetByCourseIdAsync(
        int courseId)
    {
        return await _context.Set<Chapter>()
            .Where(x => x.CourseId == courseId)
            .OrderBy(x => x.Order)
            .ToListAsync();
    }

    public async Task<Chapter?> GetByCourseIdAndOrderAsync(
        int courseId,
        int order)
    {
        return await _context.Set<Chapter>()
            .FirstOrDefaultAsync(x =>
                x.CourseId == courseId &&
                x.Order == order);
    }

    public async Task<Chapter> AddAsync(Chapter chapter)
    {
        ArgumentNullException.ThrowIfNull(chapter);

        await _context.Set<Chapter>()
            .AddAsync(chapter);

        return chapter;
    }

    public Task UpdateAsync(Chapter chapter)
    {
        ArgumentNullException.ThrowIfNull(chapter);

        _context.Set<Chapter>()
            .Update(chapter);

        return Task.CompletedTask;
    }
}