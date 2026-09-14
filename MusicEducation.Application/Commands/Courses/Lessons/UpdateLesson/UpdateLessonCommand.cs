namespace MusicEducation.Application.Commands.Courses.Lessons.UpdateLesson;

public record UpdateLessonCommand(
    int LessonId,
    string Title,
    string? Description,
    int Order
);