using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Learning;

public class LessonProgress : BaseEntity
{
    public int UserId { get; private set; }

    public int LessonId { get; private set; }

    public int? LastViewedMediaId { get; private set; }

    public TimeSpan LastPosition { get; private set; }

    public decimal ProgressPercentage { get; private set; }

    public bool IsCompleted { get; private set; }

    public DateTime LastViewedAt { get; private set; }

    private LessonProgress() { }

    public static LessonProgress Create(int userId, int lessonId)
    {
        ValidateUserId(userId);
        ValidateLessonId(lessonId);

        return new LessonProgress
        {
            UserId = userId,
            LessonId = lessonId,
            LastViewedMediaId = null,
            LastPosition = TimeSpan.Zero,
            ProgressPercentage = 0,
            IsCompleted = false,
            LastViewedAt = DateTime.UtcNow
        };
    }

    public void UpdateProgress(int mediaId, TimeSpan position, decimal progressPercentage)
    {
        ValidateMediaId(mediaId);
        ValidatePosition(position);
        ValidateProgressPercentage(progressPercentage);

        LastViewedMediaId = mediaId;
        LastPosition = position;
        ProgressPercentage = progressPercentage;
        LastViewedAt = DateTime.UtcNow;

        if (progressPercentage >= 100)
            IsCompleted = true;

        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
        ProgressPercentage = 100;
        LastViewedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reset()
    {
        LastViewedMediaId = null;
        LastPosition = TimeSpan.Zero;
        ProgressPercentage = 0;
        IsCompleted = false;
        LastViewedAt = DateTime.UtcNow;

        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateUserId(int userId)
    {
        if (userId <= 0)
            throw new DomainException("شناسه کاربر معتبر نیست", nameof(userId));
    }

    private static void ValidateLessonId(int lessonId)
    {
        if (lessonId <= 0)
            throw new DomainException("شناسه جلسه معتبر نیست", nameof(lessonId));
    }

    private static void ValidateMediaId(int mediaId)
    {
        if (mediaId <= 0)
            throw new DomainException("شناسه محتوای آموزشی معتبر نیست", nameof(mediaId));
    }

    private static void ValidatePosition(TimeSpan position)
    {
        if (position < TimeSpan.Zero)
            throw new DomainException("موقعیت مشاهده نمی‌تواند منفی باشد", nameof(position));
    }

    private static void ValidateProgressPercentage(
        decimal progressPercentage)
    {
        if (progressPercentage < 0 || progressPercentage > 100)
            throw new DomainException("درصد پیشرفت باید بین صفر تا صد باشد", nameof(progressPercentage));
    }
}