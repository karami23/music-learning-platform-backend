namespace MusicEducation.API.DTOs.Courses.Chapter;

public sealed record CreateChapterRequest(
    int CourseId,
    string Title,
    string? Description,
    int Order);

public sealed record UpdateChapterRequest(
    string Title,
    string? Description,
    int Order);

public sealed record ChangeChapterOrderRequest(
    int Order);