using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Domain.Interfaces.Courses;

public interface ICourseCategoryRepository
{
    Task<CourseCategory?> GetByIdAsync(int id);

    Task<IEnumerable<CourseCategory>> GetAllAsync();

    Task<IEnumerable<CourseCategory>> GetActiveAsync();

    Task<CourseCategory> AddAsync(CourseCategory category);

    Task<bool> ExistsByNameAsync(string name, int? excludeCategoryId = null);
    Task UpdateAsync(CourseCategory category);
}