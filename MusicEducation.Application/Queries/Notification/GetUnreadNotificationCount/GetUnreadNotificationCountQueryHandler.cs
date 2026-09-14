using MusicEducation.Domain.Interfaces.Notifications;

namespace MusicEducation.Application.Queries.Notifications.GetUnreadNotificationCount;

public class GetUnreadNotificationCountQueryHandler
{
    private readonly INotificationRepository _notificationRepository;

    public GetUnreadNotificationCountQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<int> Handle(GetUnreadNotificationCountQuery query)
    {
        return await _notificationRepository.GetUnreadCountAsync(query.UserId);
    }
}