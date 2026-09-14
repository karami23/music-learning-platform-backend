using MusicEducation.Application.DTOs.Notifications;
using MusicEducation.Application.Mappings.Notifications;
using MusicEducation.Domain.Interfaces.Notifications;

namespace MusicEducation.Application.Queries.Notifications.GetNotificationsByUserId;

public class GetNotificationsByUserIdQueryHandler
{
    private readonly INotificationRepository _notificationRepository;

    public GetNotificationsByUserIdQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<IEnumerable<NotificationDto>> Handle(GetNotificationsByUserIdQuery query)
    {
        var notifications = await _notificationRepository.GetByUserIdAsync(query.UserId);

        return notifications.Select(x => x.ToDto());
    }
}