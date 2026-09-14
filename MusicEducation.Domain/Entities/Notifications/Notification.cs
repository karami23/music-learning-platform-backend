using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Notifications;

public class Notification : BaseEntity
{
    public int UserId { get; private set; }

    public NotificationType Type { get; private set; }

    public string Title { get; private set; } = null!;

    public string Message { get; private set; } = null!;

    public string? ActionUrl { get; private set; }

    public bool IsRead { get; private set; }

    public DateTime? ReadAt { get; private set; }

    private Notification() { }

    public static Notification Create(
        int userId,
        NotificationType type,
        string title,
        string message,
        string? actionUrl = null)
    {
        ValidateUserId(userId);
        ValidateNotificationType(type);

        return new Notification
        {
            UserId = userId,
            Type = type,
            Title = NormalizeTitle(title),
            Message = NormalizeMessage(message),
            ActionUrl = NormalizeActionUrl(actionUrl),
            IsRead = false,
            ReadAt = null
        };
    }

    public void Update(
        NotificationType type,
        string title,
        string message,
        string? actionUrl)
    {
        ValidateNotificationType(type);

        Type = type;
        Title = NormalizeTitle(title);
        Message = NormalizeMessage(message);
        ActionUrl = NormalizeActionUrl(actionUrl);

        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsRead()
    {
        if (IsRead)
            return;

        IsRead = true;
        ReadAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsUnread()
    {
        if (!IsRead)
            return;

        IsRead = false;
        ReadAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateUserId(int userId)
    {
        if (userId <= 0)
            throw new DomainException("شناسه کاربر معتبر نیست", nameof(userId));
    }

    private static void ValidateNotificationType(NotificationType type)
    {
        if (!Enum.IsDefined(type))
            throw new DomainException("نوع اعلان معتبر نیست", nameof(type));
    }

    private static string NormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("عنوان اعلان الزامی است", nameof(title));

        var normalized = title.Trim();

        if (normalized.Length > DomainConstants.MaxNotificationTitleLength)
            throw new DomainException(
                $"عنوان اعلان نمی‌تواند بیشتر از {DomainConstants.MaxNotificationTitleLength} کاراکتر باشد",
                nameof(title));

        return normalized;
    }

    private static string NormalizeMessage(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new DomainException("متن اعلان الزامی است", nameof(message));

        var normalized = message.Trim();

        if (normalized.Length > DomainConstants.MaxNotificationMessageLength)
            throw new DomainException(
                $"متن اعلان نمی‌تواند بیشتر از {DomainConstants.MaxNotificationMessageLength} کاراکتر باشد",
                nameof(message));

        return normalized;
    }

    private static string? NormalizeActionUrl(string? actionUrl)
    {
        if (string.IsNullOrWhiteSpace(actionUrl))
            return null;

        var normalized = actionUrl.Trim();

        if (normalized.Length > DomainConstants.MaxNotificationActionUrlLength)
            throw new DomainException(
                $"آدرس اعلان نمی‌تواند بیشتر از {DomainConstants.MaxNotificationActionUrlLength} کاراکتر باشد",
                nameof(actionUrl));

        return normalized;
    }
}