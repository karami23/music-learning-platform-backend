using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Interfaces.Courses;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Courses;

public class CourseRepository : ICourseRepository
{
    private readonly MusicEducationDbContext _context;

    public CourseRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Set<Course>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Course?> GetBySlugAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return null;

        return await _context.Set<Course>()
            .FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<Course> AddAsync(Course course)
    {
        ArgumentNullException.ThrowIfNull(course);

        await _context.Set<Course>()
            .AddAsync(course);

        return course;
    }

    public Task UpdateAsync(Course course)
    {
        ArgumentNullException.ThrowIfNull(course);

        _context.Set<Course>()
            .Update(course);

        return Task.CompletedTask;
    }

    public async Task<bool> ExistsBySlugAsync(
        string slug,
        int? excludeCourseId = null)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return false;

        var query = _context.Set<Course>()
            .AsQueryable();

        if (excludeCourseId.HasValue)
        {
            query = query.Where(x => x.Id != excludeCourseId.Value);
        }

        return await query.AnyAsync(x => x.Slug == slug);
    }
}