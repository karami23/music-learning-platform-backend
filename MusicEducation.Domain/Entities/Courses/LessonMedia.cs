using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Courses;

public class LessonMedia : BaseEntity
{
    public int LessonId { get; private set; }

    public LessonMediaType MediaType { get; private set; }

    public string Title { get; private set; } = null!;

    public string FileName { get; private set; } = null!;

    public string StorageKey { get; private set; } = null!;

    public int Order { get; private set; }

    public TimeSpan? Duration { get; private set; }

    private LessonMedia() { }

    public static LessonMedia Create(
        int lessonId,
        LessonMediaType mediaType,
        string title,
        string fileName,
        string storageKey,
        int order,
        TimeSpan? duration = null)
    {
        ValidateLessonId(lessonId);
        ValidateMediaType(mediaType);
        ValidateOrder(order);

        var normalizedTitle = NormalizeTitle(title);
        var normalizedFileName = NormalizeFileName(fileName);
        var normalizedStorageKey = NormalizeStorageKey(storageKey);

        ValidateDuration(mediaType, duration);

        return new LessonMedia
        {
            LessonId = lessonId,
            MediaType = mediaType,
            Title = normalizedTitle,
            FileName = normalizedFileName,
            StorageKey = normalizedStorageKey,
            Order = order,
            Duration = duration
        };
    }

    public void Update(
        string title,
        string fileName,
        string storageKey,
        int order,
        TimeSpan? duration)
    {
        ValidateOrder(order);

        var normalizedTitle = NormalizeTitle(title);
        var normalizedFileName = NormalizeFileName(fileName);
        var normalizedStorageKey = NormalizeStorageKey(storageKey);

        ValidateDuration(MediaType, duration);

        Title = normalizedTitle;
        FileName = normalizedFileName;
        StorageKey = normalizedStorageKey;
        Order = order;
        Duration = duration;

        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeOrder(int order)
    {
        ValidateOrder(order);

        Order = order;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateLessonId(int lessonId)
    {
        if (lessonId <= 0)
            throw new DomainException("شناسه جلسه معتبر نیست", nameof(lessonId));
    }

    private static void ValidateMediaType(LessonMediaType mediaType)
    {
        if (!Enum.IsDefined(mediaType))
            throw new DomainException("نوع محتوای آموزشی معتبر نیست", nameof(mediaType));
    }

    private static void ValidateOrder(int order)
    {
        if (order <= 0)
            throw new DomainException("ترتیب محتوا باید بزرگ‌تر از صفر باشد", nameof(order));
    }

    private static void ValidateDuration(
        LessonMediaType mediaType,
        TimeSpan? duration)
    {
        if (duration is not null && duration <= TimeSpan.Zero)
            throw new DomainException("مدت زمان محتوا باید بیشتر از صفر باشد", nameof(duration));

        if (mediaType == LessonMediaType.Pdf && duration is not null)
            throw new DomainException("فایل PDF نمی‌تواند مدت زمان داشته باشد", nameof(duration));
    }

    private static string NormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("عنوان محتوا الزامی است", nameof(title));

        var normalized = title.Trim();

        if (normalized.Length > DomainConstants.MaxLessonMediaTitleLength)
            throw new DomainException(
                $"عنوان محتوا نمی‌تواند بیشتر از {DomainConstants.MaxLessonMediaTitleLength} کاراکتر باشد",
                nameof(title));

        return normalized;
    }

    private static string NormalizeFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new DomainException("نام فایل الزامی است", nameof(fileName));

        var normalized = fileName.Trim();

        if (normalized.Length > DomainConstants.MaxLessonMediaFileNameLength)
            throw new DomainException(
                $"نام فایل نمی‌تواند بیشتر از {DomainConstants.MaxLessonMediaFileNameLength} کاراکتر باشد",
                nameof(fileName));

        return normalized;
    }

    private static string NormalizeStorageKey(string storageKey)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
            throw new DomainException("مسیر ذخیره‌سازی فایل الزامی است", nameof(storageKey));

        var normalized = storageKey.Trim();

        if (normalized.Length > DomainConstants.MaxLessonMediaStorageKeyLength)
            throw new DomainException(
                $"مسیر ذخیره‌سازی نمی‌تواند بیشتر از {DomainConstants.MaxLessonMediaStorageKeyLength} کاراکتر باشد",
                nameof(storageKey));

        return normalized;
    }
}