using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Interfaces.Courses;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Courses;

public class LessonRepository : ILessonRepository
{
    private readonly MusicEducationDbContext _context;

    public LessonRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<Lesson?> GetByIdAsync(int id)
    {
        return await _context.Set<Lesson>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Lesson>> GetAllAsync()
    {
        return await _context.Set<Lesson>()
            .ToListAsync();
    }

    public async Task<IEnumerable<Lesson>> GetByChapterIdAsync(
        int chapterId)
    {
        return await _context.Set<Lesson>()
            .Where(x => x.ChapterId == chapterId)
            .OrderBy(x => x.Order)
            .ToListAsync();
    }

    public async Task<Lesson?> GetByChapterIdAndOrderAsync(
        int chapterId,
        int order)
    {
        return await _context.Set<Lesson>()
            .FirstOrDefaultAsync(x =>
                x.ChapterId == chapterId &&
                x.Order == order);
    }

    public async Task<Lesson?> GetFirstLessonAsync(int chapterId)
    {
        return await _context.Set<Lesson>()
            .Where(x => x.ChapterId == chapterId)
            .OrderBy(x => x.Order)
            .FirstOrDefaultAsync();
    }

    public async Task<Lesson?> GetNextLessonAsync(int lessonId)
    {
        var lesson = await _context.Set<Lesson>()
            .FirstOrDefaultAsync(x => x.Id == lessonId);

        if (lesson is null)
            return null;

        return await _context.Set<Lesson>()
            .Where(x =>
                x.ChapterId == lesson.ChapterId &&
                x.Order > lesson.Order)
            .OrderBy(x => x.Order)
            .FirstOrDefaultAsync();
    }

    public async Task<Lesson> AddAsync(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        await _context.Set<Lesson>()
            .AddAsync(lesson);

        return lesson;
    }

    public Task UpdateAsync(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        _context.Set<Lesson>()
            .Update(lesson);

        return Task.CompletedTask;
    }
}