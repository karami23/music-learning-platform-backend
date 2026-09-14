using MusicEducation.Application.DTOs.Notifications;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Mappings.Notifications;
using MusicEducation.Domain.Entities.Notifications;
using MusicEducation.Domain.Interfaces.Notifications;

namespace MusicEducation.Application.Commands.Notifications.CreateNotification;

public sealed class CreateNotificationCommandHandler
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateNotificationCommandHandler(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<NotificationDto> Handle(
        CreateNotificationCommand command,
        CancellationToken cancellationToken = default)
    {
        var notification = Notification.Create(
            command.UserId,
            command.Type,
            command.Title,
            command.Message,
            command.ActionUrl);

        var createdNotification =
            await _notificationRepository.AddAsync(notification);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return createdNotification.ToDto();
    }
}