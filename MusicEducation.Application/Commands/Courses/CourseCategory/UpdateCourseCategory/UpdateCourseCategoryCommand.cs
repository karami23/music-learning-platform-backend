namespace MusicEducation.Application.Commands.Courses.CourseCategory.UpdateCourseCategory;

public sealed record UpdateCourseCategoryCommand(
    int CategoryId,
    string Name,
    string? Description
);