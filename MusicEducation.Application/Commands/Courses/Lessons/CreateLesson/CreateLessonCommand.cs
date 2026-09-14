namespace MusicEducation.Application.Commands.Courses.Lessons.CreateLesson;

public record CreateLessonCommand(
    int ChapterId,
    string Title,
    string? Description,
    int Order
);