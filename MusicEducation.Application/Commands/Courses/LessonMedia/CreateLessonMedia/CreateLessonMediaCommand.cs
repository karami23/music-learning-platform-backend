using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Commands.Courses.LessonMedia.CreateLessonMedia;

public record CreateLessonMediaCommand(
    int LessonId,
    LessonMediaType MediaType,
    string Title,
    string FileName,
    string StorageKey,
    int Order,
    TimeSpan? Duration
);