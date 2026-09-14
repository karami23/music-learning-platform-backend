using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Entities.Learning;
using MusicEducation.Domain.Interfaces.Learning;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Learning;

public class LessonProgressRepository : ILessonProgressRepository
{
    private readonly MusicEducationDbContext _context;

    public LessonProgressRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<LessonProgress?> GetByIdAsync(int id)
    {
        return await _context.Set<LessonProgress>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<LessonProgress?> GetByUserIdAndLessonIdAsync(
        int userId,
        int lessonId)
    {
        return await _context.Set<LessonProgress>()
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.LessonId == lessonId);
    }

    public async Task<IEnumerable<LessonProgress>> GetByUserIdAsync(int userId)
    {
        return await _context.Set<LessonProgress>()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.LastViewedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<LessonProgress>> GetByLessonIdAsync(int lessonId)
    {
        return await _context.Set<LessonProgress>()
            .Where(x => x.LessonId == lessonId)
            .OrderByDescending(x => x.LastViewedAt)
            .ToListAsync();
    }

    public async Task<LessonProgress?> GetLastProgressAsync(
        int userId,
        int courseId)
    {
        return await _context.Set<LessonProgress>()
            .Where(x =>
                x.UserId == userId &&
                _context.Set<Lesson>()
                    .Any(lesson =>
                        lesson.Id == x.LessonId &&
                        _context.Set<Chapter>()
                            .Any(chapter =>
                                chapter.Id == lesson.ChapterId &&
                                chapter.CourseId == courseId)))
            .OrderByDescending(x => x.LastViewedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<LessonProgress> AddAsync(LessonProgress progress)
    {
        ArgumentNullException.ThrowIfNull(progress);

        await _context.Set<LessonProgress>()
            .AddAsync(progress);

        return progress;
    }

    public Task UpdateAsync(LessonProgress progress)
    {
        ArgumentNullException.ThrowIfNull(progress);

        _context.Set<LessonProgress>()
            .Update(progress);

        return Task.CompletedTask;
    }
}