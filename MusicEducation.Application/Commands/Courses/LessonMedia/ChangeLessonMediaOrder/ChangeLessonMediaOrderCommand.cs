namespace MusicEducation.Application.Commands.Courses.LessonMedia.ChangeLessonMediaOrder;

public record ChangeLessonMediaOrderCommand(
    int LessonMediaId,
    int Order
);