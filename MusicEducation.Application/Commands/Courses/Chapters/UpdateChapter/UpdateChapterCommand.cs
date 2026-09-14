namespace MusicEducation.Application.Commands.Courses.Chapters.UpdateChapter;

public sealed record UpdateChapterCommand(
    int ChapterId,
    string Title,
    string? Description,
    int Order
);