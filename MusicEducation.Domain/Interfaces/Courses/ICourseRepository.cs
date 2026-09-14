using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Domain.Interfaces.Courses;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(int id);

    Task<Course?> GetBySlugAsync(string slug);

    Task<Course> AddAsync(Course course);

    Task UpdateAsync(Course course);

    Task<bool> ExistsBySlugAsync(
        string slug,
        int? excludeCourseId = null);
}