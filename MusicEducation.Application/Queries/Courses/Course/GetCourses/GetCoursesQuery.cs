namespace MusicEducation.Application.Queries.Courses;

public sealed record GetCoursesQuery(
    string? Search,
    int? TeacherId,
    int? CourseCategoryId,
    int Page = 1,
    int PageSize = 10
);