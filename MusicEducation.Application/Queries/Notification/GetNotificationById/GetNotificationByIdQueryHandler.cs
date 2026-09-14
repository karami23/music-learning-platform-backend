using MusicEducation.Application.DTOs.Notifications;
using MusicEducation.Application.Mappings.Notifications;
using MusicEducation.Domain.Interfaces.Notifications;

namespace MusicEducation.Application.Queries.Notifications.GetNotificationById;

public class GetNotificationByIdQueryHandler
{
    private readonly INotificationRepository _notificationRepository;

    public GetNotificationByIdQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<NotificationDto?> Handle(GetNotificationByIdQuery query)
    {
        var notification = await _notificationRepository.GetByIdAsync(query.NotificationId);

        return notification?.ToDto();
    }
}