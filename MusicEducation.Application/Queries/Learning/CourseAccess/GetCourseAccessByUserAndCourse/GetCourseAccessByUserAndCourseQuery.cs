namespace MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessByUserAndCourse;

public record GetCourseAccessByUserAndCourseQuery(
    int UserId,
    int CourseId
);