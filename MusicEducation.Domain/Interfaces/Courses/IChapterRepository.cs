using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Domain.Interfaces.Courses;

public interface IChapterRepository
{
    Task<Chapter?> GetByIdAsync(int id);

    Task<IEnumerable<Chapter>> GetAllAsync();

    Task<IEnumerable<Chapter>> GetByCourseIdAsync(int courseId);

    Task<Chapter?> GetByCourseIdAndOrderAsync(int courseId, int order);

    Task<Chapter> AddAsync(Chapter chapter);

    Task UpdateAsync(Chapter chapter);
}