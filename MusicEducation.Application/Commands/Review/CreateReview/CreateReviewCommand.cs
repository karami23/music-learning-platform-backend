namespace MusicEducation.Application.Commands.Reviews.CreateReview;

public record CreateReviewCommand(
    int UserId,
    int CourseId,
    decimal Rating,
    string? Comment
);