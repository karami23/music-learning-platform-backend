namespace MusicEducation.Application.Commands.Courses.Lessons.ChangeLessonOrder;

public record ChangeLessonOrderCommand(
    int LessonId,
    int Order
);