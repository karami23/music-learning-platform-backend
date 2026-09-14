using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Interfaces.Courses;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Courses;

public class CourseCategoryRepository : ICourseCategoryRepository
{
    private readonly MusicEducationDbContext _context;

    public CourseCategoryRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<CourseCategory?> GetByIdAsync(int id)
    {
        return await _context.Set<CourseCategory>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<CourseCategory>> GetAllAsync()
    {
        return await _context.Set<CourseCategory>()
            .ToListAsync();
    }

    public async Task<IEnumerable<CourseCategory>> GetActiveAsync()
    {
        return await _context.Set<CourseCategory>()
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task<CourseCategory> AddAsync(
        CourseCategory category)
    {
        ArgumentNullException.ThrowIfNull(category);

        await _context.Set<CourseCategory>()
            .AddAsync(category);

        return category;
    }

    public async Task<bool> ExistsByNameAsync(
        string name,
        int? excludeCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var query = _context.Set<CourseCategory>()
            .AsQueryable();

        if (excludeCategoryId.HasValue)
        {
            query = query.Where(x => x.Id != excludeCategoryId.Value);
        }

        return await query.AnyAsync(x => x.Name == name);
    }

    public Task UpdateAsync(CourseCategory category)
    {
        ArgumentNullException.ThrowIfNull(category);

        _context.Set<CourseCategory>()
            .Update(category);

        return Task.CompletedTask;
    }
}