using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Commands.Courses.LessonMedia.UpdateLessonMedia;

public record UpdateLessonMediaCommand(
    int MediaId,
    string Title,
    string FileName,
    string StorageKey,
    int Order,
    TimeSpan? Duration
);