namespace MusicEducation.Application.Commands.Notifications.MarkNotificationAsUnread;

public record MarkNotificationAsUnreadCommand(
    int UserId,
    int NotificationId);