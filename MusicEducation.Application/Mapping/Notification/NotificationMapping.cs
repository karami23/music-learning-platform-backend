using MusicEducation.Application.DTOs.Notifications;
using MusicEducation.Domain.Entities.Notifications;

namespace MusicEducation.Application.Mappings.Notifications;

public static class NotificationMapping
{
    public static NotificationDto ToDto(this Notification notification)
    {
        return new NotificationDto(
            notification.Id,
            notification.UserId,
            notification.Type,
            notification.Title,
            notification.Message,
            notification.ActionUrl,
            notification.IsRead,
            notification.ReadAt
        );
    }
}