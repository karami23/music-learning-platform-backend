using MusicEducation.Domain.Enums;

namespace MusicEducation.API.DTOs.Notifications;

public sealed record CreateNotificationRequest(
    int UserId,
    NotificationType Type,
    string Title,
    string Message,
    string? ActionUrl);

public sealed record UpdateNotificationRequest(
    NotificationType Type,
    string Title,
    string Message,
    string? ActionUrl);