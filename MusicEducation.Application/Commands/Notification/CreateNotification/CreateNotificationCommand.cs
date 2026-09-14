using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Commands.Notifications.CreateNotification;

public record CreateNotificationCommand(
    int UserId,
    NotificationType Type,
    string Title,
    string Message,
    string? ActionUrl
);