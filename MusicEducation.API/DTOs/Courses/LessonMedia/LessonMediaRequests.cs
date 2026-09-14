
namespace MusicEducation.API.DTOs.Courses.LessonMedia;

public sealed record CreateLessonMediaRequest(
    int LessonId,
    LessonMediaType MediaType,
    string Title,
    string FileName,
    string StorageKey,
    int Order,
    int? Duration);

public sealed record UpdateLessonMediaRequest(
    string Title,
    string FileName,
    string StorageKey,
    int Order,
    int? Duration);

public sealed record ChangeLessonMediaOrderRequest(
    int Order);