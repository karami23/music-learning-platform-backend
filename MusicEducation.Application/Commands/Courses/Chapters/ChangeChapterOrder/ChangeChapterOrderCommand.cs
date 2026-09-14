namespace MusicEducation.Application.Commands.Courses.Chapters.ChangeChapterOrder;

public sealed record ChangeChapterOrderCommand(
    int ChapterId,
    int Order
);