using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.DTOs.Identity;

public record TeacherDto(
    int Id,
    int UserId,
    string? Bio,
    TeacherStatus Status
);