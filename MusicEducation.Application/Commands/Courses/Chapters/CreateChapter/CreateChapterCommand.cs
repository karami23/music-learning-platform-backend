namespace MusicEducation.Application.Commands.Courses.Chapters.CreateChapter;

public sealed record CreateChapterCommand(
    int CourseId,
    string Title,
    string? Description,
    int Order
);