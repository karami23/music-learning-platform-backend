using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Courses;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Courses;

public class LessonMediaRepository : ILessonMediaRepository
{
    private readonly MusicEducationDbContext _context;

    public LessonMediaRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<LessonMedia?> GetByIdAsync(int id)
    {
        return await _context.Set<LessonMedia>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<LessonMedia>> GetAllAsync()
    {
        return await _context.Set<LessonMedia>()
            .ToListAsync();
    }

    public async Task<IEnumerable<LessonMedia>> GetByLessonIdAsync(
        int lessonId)
    {
        return await _context.Set<LessonMedia>()
            .Where(x => x.LessonId == lessonId)
            .OrderBy(x => x.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<LessonMedia>> GetByLessonIdAndTypeAsync(
        int lessonId,
        LessonMediaType mediaType)
    {
        return await _context.Set<LessonMedia>()
            .Where(x =>
                x.LessonId == lessonId &&
                x.MediaType == mediaType)
            .OrderBy(x => x.Order)
            .ToListAsync();
    }

    public async Task<LessonMedia?> GetByLessonIdAndOrderAsync(
        int lessonId,
        int order)
    {
        return await _context.Set<LessonMedia>()
            .FirstOrDefaultAsync(x =>
                x.LessonId == lessonId &&
                x.Order == order);
    }

    public async Task<LessonMedia?> GetFirstMediaAsync(int lessonId)
    {
        return await _context.Set<LessonMedia>()
            .Where(x => x.LessonId == lessonId)
            .OrderBy(x => x.Order)
            .FirstOrDefaultAsync();
    }

    public async Task<LessonMedia?> GetNextMediaAsync(int mediaId)
    {
        var media = await _context.Set<LessonMedia>()
            .FirstOrDefaultAsync(x => x.Id == mediaId);

        if (media is null)
            return null;

        return await _context.Set<LessonMedia>()
            .Where(x =>
                x.LessonId == media.LessonId &&
                x.Order > media.Order)
            .OrderBy(x => x.Order)
            .FirstOrDefaultAsync();
    }

    public async Task<LessonMedia> AddAsync(LessonMedia media)
    {
        ArgumentNullException.ThrowIfNull(media);

        await _context.Set<LessonMedia>()
            .AddAsync(media);

        return media;
    }

    public Task UpdateAsync(LessonMedia media)
    {
        ArgumentNullException.ThrowIfNull(media);

        _context.Set<LessonMedia>()
            .Update(media);

        return Task.CompletedTask;
    }
}