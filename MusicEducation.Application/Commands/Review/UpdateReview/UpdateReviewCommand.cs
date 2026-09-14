namespace MusicEducation.Application.Commands.Reviews.UpdateReview;

public record UpdateReviewCommand(
    int UserId,
    int ReviewId,
    decimal Rating,
    string? Comment
);