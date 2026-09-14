using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Exceptions;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Domain.Entities.Courses;

public class Course : BaseEntity
{
    public int TeacherId { get; private set; }

    public int CourseCategoryId { get; private set; }

    public string Title { get; private set; } = null!;

    public string Slug { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public Money? Price { get; private set; }

    public CoursePricingType PricingType { get; private set; }

    public CourseStatus Status { get; private set; }

    public string? CoverImageUrl { get; private set; }

    private Course() { }

    public static Course Create(
        int teacherId,
        int courseCategoryId,
        string title,
        string slug,
        string description,
        CoursePricingType pricingType,
        Money? price = null,
        string? coverImageUrl = null)
    {
        ValidateTeacherId(teacherId);
        ValidateCourseCategoryId(courseCategoryId);

        var normalizedTitle = NormalizeTitle(title);
        var normalizedSlug = NormalizeSlug(slug);
        var normalizedDescription = NormalizeDescription(description);
        var normalizedCoverImageUrl =
            NormalizeCoverImageUrl(coverImageUrl);

        ValidatePricing(pricingType, price);

        return new Course
        {
            TeacherId = teacherId,
            CourseCategoryId = courseCategoryId,
            Title = normalizedTitle,
            Slug = normalizedSlug,
            Description = normalizedDescription,
            PricingType = pricingType,
            Price = price,
            CoverImageUrl = normalizedCoverImageUrl,
            Status = CourseStatus.Draft
        };
    }

    public void Update(
        int teacherId,
        int courseCategoryId,
        string title,
        string slug,
        string description,
        CoursePricingType pricingType,
        Money? price,
        string? coverImageUrl)
    {
        ValidateTeacherId(teacherId);
        ValidateCourseCategoryId(courseCategoryId);
        ValidatePricing(pricingType, price);

        TeacherId = teacherId;
        CourseCategoryId = courseCategoryId;
        Title = NormalizeTitle(title);
        Slug = NormalizeSlug(slug);
        Description = NormalizeDescription(description);
        PricingType = pricingType;
        Price = price;
        CoverImageUrl = NormalizeCoverImageUrl(coverImageUrl);

        UpdatedAt = DateTime.UtcNow;
    }

    public void SubmitForReview()
    {
        if (Status != CourseStatus.Draft)
        {
            throw new DomainException(
                "فقط دوره‌های پیش‌نویس می‌توانند برای بررسی ارسال شوند");
        }

        Status = CourseStatus.PendingReview;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Publish()
    {
        if (Status != CourseStatus.PendingReview)
        {
            throw new DomainException(
                "فقط دوره‌های در انتظار بررسی می‌توانند منتشر شوند");
        }

        Status = CourseStatus.Published;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        if (Status != CourseStatus.PendingReview)
        {
            throw new DomainException(
                "فقط دوره‌های در انتظار بررسی می‌توانند رد شوند");
        }

        Status = CourseStatus.Draft;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Archive()
    {
        if (Status == CourseStatus.Archived)
            return;

        if (Status != CourseStatus.Published)
        {
            throw new DomainException(
                "فقط دوره‌های منتشرشده می‌توانند آرشیو شوند");
        }

        Status = CourseStatus.Archived;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReturnToDraft()
    {
        if (Status == CourseStatus.Draft)
            return;

        Status = CourseStatus.Draft;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePrice(Money? price)
    {
        ValidatePricing(PricingType, price);

        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePricingType(
        CoursePricingType pricingType,
        Money? price)
    {
        ValidatePricing(pricingType, price);

        PricingType = pricingType;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidatePricing(
        CoursePricingType pricingType,
        Money? price)
    {
        if (!Enum.IsDefined(pricingType))
        {
            throw new DomainException(
                "نوع قیمت‌گذاری دوره معتبر نیست",
                nameof(pricingType));
        }

        if (pricingType == CoursePricingType.Free &&
            price is not null)
        {
            throw new DomainException(
                "دوره رایگان نمی‌تواند قیمت داشته باشد",
                nameof(price));
        }

        if (pricingType == CoursePricingType.Paid &&
            price is null)
        {
            throw new DomainException(
                "قیمت دوره پولی الزامی است",
                nameof(price));
        }
    }

    private static void ValidateTeacherId(int teacherId)
    {
        if (teacherId <= 0)
        {
            throw new DomainException(
                "شناسه مدرس معتبر نیست",
                nameof(teacherId));
        }
    }

    private static void ValidateCourseCategoryId(
        int courseCategoryId)
    {
        if (courseCategoryId <= 0)
        {
            throw new DomainException(
                "شناسه دسته‌بندی معتبر نیست",
                nameof(courseCategoryId));
        }
    }

    private static string NormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException(
                "عنوان دوره الزامی است",
                nameof(title));
        }

        var normalized = title.Trim();

        if (normalized.Length >
            DomainConstants.MaxCourseTitleLength)
        {
            throw new DomainException(
                $"عنوان دوره نمی‌تواند بیشتر از " +
                $"{DomainConstants.MaxCourseTitleLength} کاراکتر باشد",
                nameof(title));
        }

        return normalized;
    }

    private static string NormalizeSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new DomainException(
                "Slug دوره الزامی است",
                nameof(slug));
        }

        var normalized = slug.Trim().ToLowerInvariant();

        if (normalized.Length > 200)
        {
            throw new DomainException(
                "Slug دوره نمی‌تواند بیشتر از 200 کاراکتر باشد",
                nameof(slug));
        }

        return normalized;
    }

    private static string NormalizeDescription(
        string description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new DomainException(
                "توضیحات دوره الزامی است",
                nameof(description));
        }

        var normalized = description.Trim();

        if (normalized.Length >
            DomainConstants.MaxCourseDescriptionLength)
        {
            throw new DomainException(
                $"توضیحات دوره نمی‌تواند بیشتر از " +
                $"{DomainConstants.MaxCourseDescriptionLength} کاراکتر باشد",
                nameof(description));
        }

        return normalized;
    }

    private static string? NormalizeCoverImageUrl(
        string? coverImageUrl)
    {
        if (string.IsNullOrWhiteSpace(coverImageUrl))
            return null;

        var normalized = coverImageUrl.Trim();

        if (normalized.Length >
            DomainConstants.MaxCourseCoverImageUrlLength)
        {
            throw new DomainException(
                $"آدرس تصویر دوره نمی‌تواند بیشتر از " +
                $"{DomainConstants.MaxCourseCoverImageUrlLength} کاراکتر باشد",
                nameof(coverImageUrl));
        }

        if (!Uri.TryCreate(
                normalized,
                UriKind.Absolute,
                out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp &&
             uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new DomainException(
                "آدرس تصویر دوره معتبر نیست",
                nameof(coverImageUrl));
        }

        return normalized;
    }
}