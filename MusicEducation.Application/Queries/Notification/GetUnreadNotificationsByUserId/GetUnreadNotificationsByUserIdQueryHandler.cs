using MusicEducation.Application.DTOs.Notifications;
using MusicEducation.Application.Mappings.Notifications;
using MusicEducation.Domain.Interfaces.Notifications;

namespace MusicEducation.Application.Queries.Notifications.GetUnreadNotificationsByUserId;

public class GetUnreadNotificationsByUserIdQueryHandler
{
    private readonly INotificationRepository _notificationRepository;

    public GetUnreadNotificationsByUserIdQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<IEnumerable<NotificationDto>> Handle(GetUnreadNotificationsByUserIdQuery query)
    {
        var notifications = await _notificationRepository.GetUnreadByUserIdAsync(query.UserId);

        return notifications.Select(x => x.ToDto());
    }
}