using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.DTOs.Notifications;

public record NotificationDto(
    int Id,
    int UserId,
    NotificationType Type,
    string Title,
    string Message,
    string? ActionUrl,
    bool IsRead,
    DateTime? ReadAt
);