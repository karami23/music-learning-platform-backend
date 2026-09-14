namespace MusicEducation.Application.Commands.Learning.CourseAccess.CreateCourseAccess;

public record CreateCourseAccessCommand(
    int UserId,
    int CourseId
);