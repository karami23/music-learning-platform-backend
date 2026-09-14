namespace MusicEducation.Application.Queries.Reviews.GetReviewByUserAndCourse;

public record GetReviewByUserAndCourseQuery(
    int UserId,
    int CourseId
);