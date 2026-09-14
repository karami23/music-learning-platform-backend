namespace MusicEducation.Application.Commands.Courses.CourseCategory.CreateCourseCategory;

public sealed record CreateCourseCategoryCommand(
    string Name,
    string? Description
);