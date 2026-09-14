namespace MusicEducation.Application.Commands.Courses.SubmitCourseForReview;

public sealed record SubmitCourseForReviewCommand(
    int UserId,
    int CourseId);