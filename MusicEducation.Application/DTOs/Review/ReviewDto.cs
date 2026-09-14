using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.DTOs.Reviews;

public record ReviewDto(
    int Id,
    int UserId,
    int CourseId,
    decimal Rating,
    string? Comment,
    ReviewStatus Status,
    DateTime? ReviewedAt
);