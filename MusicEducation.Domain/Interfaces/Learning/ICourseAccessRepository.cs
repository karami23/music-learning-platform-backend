using MusicEducation.Domain.Entities.Learning;

namespace MusicEducation.Domain.Interfaces.Learning;

public interface ICourseAccessRepository
{
    Task<CourseAccess?> GetByIdAsync(int id);

    Task<CourseAccess?> GetByUserIdAndCourseIdAsync(
        int userId,
        int courseId);

    Task<IEnumerable<CourseAccess>> GetByUserIdAsync(int userId);

    Task<IEnumerable<CourseAccess>> GetByCourseIdAsync(int courseId);

    Task<bool> HasAccessAsync(int userId, int courseId);

    Task<CourseAccess> AddAsync(CourseAccess courseAccess);

    Task UpdateAsync(CourseAccess courseAccess);
}