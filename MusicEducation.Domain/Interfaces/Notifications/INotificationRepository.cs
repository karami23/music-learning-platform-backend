using MusicEducation.Domain.Entities.Notifications;

namespace MusicEducation.Domain.Interfaces.Notifications;

public interface INotificationRepository
{
    Task<Notification?> GetByIdAsync(int id);

    Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);

    Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(int userId);

    Task<int> GetUnreadCountAsync(int userId);

    Task<Notification> AddAsync(Notification notification);

    Task UpdateAsync(Notification notification);
}