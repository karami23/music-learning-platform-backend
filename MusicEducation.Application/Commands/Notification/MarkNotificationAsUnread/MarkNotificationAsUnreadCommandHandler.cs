using MusicEducation.Application.DTOs.Notifications;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Mappings.Notifications;
using MusicEducation.Domain.Interfaces.Notifications;

namespace MusicEducation.Application.Commands.Notifications.MarkNotificationAsUnread;

public sealed class MarkNotificationAsUnreadCommandHandler
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkNotificationAsUnreadCommandHandler(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<NotificationDto> Handle(
        MarkNotificationAsUnreadCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (command.NotificationId <= 0)
            throw new ValidationException(
                "شناسه اعلان معتبر نیست");

        var notification = await _notificationRepository
            .GetByIdAsync(command.NotificationId);

        if (notification is null)
            throw new NotFoundException(
                "اعلان مورد نظر پیدا نشد");

        if (notification.UserId != command.UserId)
            throw new UnauthorizedException(
                "شما دسترسی به این اعلان را ندارید");

        notification.MarkAsUnread();

        await _notificationRepository.UpdateAsync(notification);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return notification.ToDto();
    }
}