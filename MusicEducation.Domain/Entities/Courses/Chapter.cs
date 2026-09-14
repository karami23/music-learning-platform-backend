using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Courses;

public class Chapter : BaseEntity
{
    public int CourseId { get; private set; }

    public string Title { get; private set; } = null!;

    public string? Description { get; private set; }

    public int Order { get; private set; }

    private Chapter() { }

    public static Chapter Create(
        int courseId,
        string title,
        string? description,
        int order)
    {
        ValidateCourseId(courseId);
        ValidateOrder(order);

        return new Chapter
        {
            CourseId = courseId,
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

    private static void ValidateCourseId(int courseId)
    {
        if (courseId <= 0)
            throw new DomainException("شناسه دوره معتبر نیست", nameof(courseId));
    }

    private static void ValidateOrder(int order)
    {
        if (order <= 0)
            throw new DomainException("ترتیب فصل باید بزرگ‌تر از صفر باشد", nameof(order));
    }

    private static string NormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("عنوان فصل الزامی است", nameof(title));

        var normalized = title.Trim();

        if (normalized.Length > DomainConstants.MaxChapterTitleLength)
            throw new DomainException(
                $"عنوان فصل نمی‌تواند بیشتر از {DomainConstants.MaxChapterTitleLength} کاراکتر باشد",
                nameof(title));

        return normalized;
    }

    private static string? NormalizeDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return null;

        var normalized = description.Trim();

        if (normalized.Length > DomainConstants.MaxChapterDescriptionLength)
            throw new DomainException(
                $"توضیحات فصل نمی‌تواند بیشتر از {DomainConstants.MaxChapterDescriptionLength} کاراکتر باشد",
                nameof(description));

        return normalized;
    }
}