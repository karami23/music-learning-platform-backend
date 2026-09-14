using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Identity;

public class TeacherRepository : ITeacherRepository
{
    private readonly MusicEducationDbContext _context;

    public TeacherRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<Teacher?> GetByIdAsync(int id)
    {
        return await _context.Set<Teacher>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Teacher?> GetByUserIdAsync(int userId)
    {
        return await _context.Set<Teacher>()
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<IEnumerable<Teacher>> GetAllAsync()
    {
        return await _context.Set<Teacher>()
            .ToListAsync();
    }

    public async Task<IEnumerable<Teacher>> GetActiveTeachersAsync()
    {
        return await _context.Set<Teacher>()
            .Where(x => x.Status == TeacherStatus.Active)
            .ToListAsync();
    }

    public async Task<Teacher> AddAsync(Teacher teacher)
    {
        ArgumentNullException.ThrowIfNull(teacher);

        await _context.Set<Teacher>().AddAsync(teacher);

        return teacher;
    }

    public Task UpdateAsync(Teacher teacher)
    {
        ArgumentNullException.ThrowIfNull(teacher);

        _context.Set<Teacher>().Update(teacher);

        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByUserIdAsync(int userId)
    {
        return await _context.Set<Teacher>()
            .AnyAsync(x => x.UserId == userId);
    }
}