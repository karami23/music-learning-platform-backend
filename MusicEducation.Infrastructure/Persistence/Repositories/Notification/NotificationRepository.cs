using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Notifications;
using MusicEducation.Domain.Interfaces.Notifications;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Notifications;

public class NotificationRepository : INotificationRepository
{
    private readonly MusicEducationDbContext _context;

    public NotificationRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        return await _context.Set<Notification>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
    {
        return await _context.Set<Notification>()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(
        int userId)
    {
        return await _context.Set<Notification>()
            .Where(x =>
                x.UserId == userId &&
                !x.IsRead)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await _context.Set<Notification>()
            .CountAsync(x =>
                x.UserId == userId &&
                !x.IsRead);
    }

    public async Task<Notification> AddAsync(Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);

        await _context.Set<Notification>()
            .AddAsync(notification);

        return notification;
    }

    public Task UpdateAsync(Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification);

        _context.Set<Notification>()
            .Update(notification);

        return Task.CompletedTask;
    }
}