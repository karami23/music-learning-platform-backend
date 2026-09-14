using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.DTOs.Learning;

public record CourseAccessDto(
    int Id,
    int UserId,
    int CourseId,
    CourseAccessStatus Status,
    DateTime GrantedAt,
    DateTime? RevokedAt
);