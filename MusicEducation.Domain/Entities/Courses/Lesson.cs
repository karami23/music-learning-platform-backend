using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Courses;

public class Lesson : BaseEntity
{
    public int ChapterId { get; private set; }

    public string Title { get; private set; } = null!;

    public string? Description { get; private set; }

    public int Order { get; private set; }

    private readonly List<LessonMedia> _media = new();

    public IReadOnlyCollection<LessonMedia> Media => _media.AsReadOnly();

    private Lesson() { }

    public static Lesson Create(
        int chapterId,
        string title,
        string? description,
        int order)
    {
        ValidateChapterId(chapterId);
        ValidateOrder(order);

        return new Lesson
        {
            ChapterId = chapterId,
            Title = NormalizeTitle(title),
            Description = NormalizeDescription(description),
            Order = order
        };
    }

    public void Update(
        string title,
        string? description,
        int order)
    {
        ValidateOrder(order);

        Title = NormalizeTitle(title);
        Description = NormalizeDescription(description);
        Order = order;

        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeOrder(int order)
    {
        ValidateOrder(order);

        Order = order;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddMedia(LessonMedia media)
    {
        ArgumentNullException.ThrowIfNull(media);

        _media.Add(media);

        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveMedia(LessonMedia media)
    {
        ArgumentNullException.ThrowIfNull(media);

        if (!_media.Remove(media))
            throw new DomainException("محتوای موردنظر در این جلسه وجود ندارد", nameof(media));

        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateChapterId(int chapterId)
    {
        if (chapterId <= 0)
            throw new DomainException("شناسه فصل معتبر نیست", nameof(chapterId));
    }

    private static void ValidateOrder(int order)
    {
        if (order <= 0)
            throw new DomainException("ترتیب جلسه باید بزرگ‌تر از صفر باشد", nameof(order));
    }

    private static string NormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("عنوان جلسه الزامی است", nameof(title));

        var normalized = title.Trim();

        if (normalized.Length > DomainConstants.MaxLessonTitleLength)
            throw new DomainException(
                $"عنوان جلسه نمی‌تواند بیشتر از {DomainConstants.MaxLessonTitleLength} کاراکتر باشد",
                nameof(title));

        return normalized;
    }

    private static string? NormalizeDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return null;

        var normalized = description.Trim();

        if (normalized.Length > DomainConstants.MaxLessonDescriptionLength)
            throw new DomainException(
                $"توضیحات جلسه نمی‌تواند بیشتر از {DomainConstants.MaxLessonDescriptionLength} کاراکتر باشد",
                nameof(description));

        return normalized;
    }
}