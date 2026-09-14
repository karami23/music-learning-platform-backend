namespace MusicEducation.API.DTOs.Reviews;

public sealed record CreateReviewRequest(
    int CourseId,
    decimal Rating,
    string? Comment);

public sealed record UpdateReviewRequest(
    decimal Rating,
    string? Comment);