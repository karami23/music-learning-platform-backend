namespace MusicEducation.Application.Commands.Notifications.MarkNotificationAsRead;

public record MarkNotificationAsReadCommand(
    int UserId,
    int NotificationId);