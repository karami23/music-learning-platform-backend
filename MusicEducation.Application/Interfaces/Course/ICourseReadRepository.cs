namespace MusicEducation.Application.Interfaces.Courses;

public interface ICourseReadRepository
{
    Task<IEnumerable<CourseListReadModel>> GetPagedAsync(
        string? search,
        int? teacherId,
        int? courseCategoryId,
        int page,
        int pageSize);

    Task<CourseDetailsReadModel?> GetBySlugAsync(string slug);

    Task<CourseDetailsReadModel?> GetByIdAsync(int id);
}