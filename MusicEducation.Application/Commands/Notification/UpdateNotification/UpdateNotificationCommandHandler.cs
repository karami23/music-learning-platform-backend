using MusicEducation.Application.DTOs.Notifications;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Mappings.Notifications;
using MusicEducation.Domain.Interfaces.Notifications;

namespace MusicEducation.Application.Commands.Notifications.UpdateNotification;

public sealed class UpdateNotificationCommandHandler
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateNotificationCommandHandler(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<NotificationDto> Handle(
        UpdateNotificationCommand command,
        CancellationToken cancellationToken = default)
    {
        var notification = await _notificationRepository
            .GetByIdAsync(command.NotificationId);

        if (notification is null)
            throw new NotFoundException("اعلان مورد نظر پیدا نشد");

        notification.Update(
            command.Type,
            command.Title,
            command.Message,
            command.ActionUrl);

        await _notificationRepository.UpdateAsync(notification);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return notification.ToDto();
    }
}