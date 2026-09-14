namespace MusicEducation.API.DTOs.Courses.Lesson;

public sealed record CreateLessonRequest(
    int ChapterId,
    string Title,
    string? Description,
    int Order);

public sealed record UpdateLessonRequest(
    string Title,
    string? Description,
    int Order);

public sealed record ChangeLessonOrderRequest(
    int Order);