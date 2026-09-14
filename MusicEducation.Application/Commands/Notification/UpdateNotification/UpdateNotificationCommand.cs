using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Commands.Notifications.UpdateNotification;

public record UpdateNotificationCommand(
    int NotificationId,
    NotificationType Type,
    string Title,
    string Message,
    string? ActionUrl
);