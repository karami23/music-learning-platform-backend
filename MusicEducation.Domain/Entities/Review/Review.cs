using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Reviews;

public class Review : BaseEntity
{
    public int UserId { get; private set; }

    public int CourseId { get; private set; }

    public decimal Rating { get; private set; }
    public string? Comment { get; private set; }

    public ReviewStatus Status { get; private set; }

    public DateTime? ReviewedAt { get; private set; }

    private Review() { }
    public static Review Create(
        int userId,
        int courseId,
        decimal rating,
        string? comment)
    {
        ValidateUserId(userId);
        ValidateCourseId(courseId);
        ValidateRating(rating);

        return new Review
        {
            UserId = userId,
            CourseId = courseId,
            Rating = rating,
            Comment = NormalizeComment(comment),
            Status = ReviewStatus.Pending,
            ReviewedAt = null
        };
    }

    public void Update(decimal rating, string? comment)
    {
        if (Status == ReviewStatus.Approved)
            throw new DomainException("نظر تأییدشده قابل ویرایش نیست", nameof(Status));

        ValidateRating(rating);

        Rating = rating;
        Comment = NormalizeComment(comment);
        Status = ReviewStatus.Pending;
        ReviewedAt = null;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Approve()
    {
        if (Status == ReviewStatus.Approved)
            return;

        Status = ReviewStatus.Approved;
        ReviewedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reject()
    {
        if (Status == ReviewStatus.Rejected)
            return;

        Status = ReviewStatus.Rejected;
        ReviewedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateUserId(int userId)
    {
        if (userId <= 0)
            throw new DomainException("شناسه کاربر معتبر نیست", nameof(userId));
    }

    private static void ValidateCourseId(int courseId)
    {
        if (courseId <= 0)
            throw new DomainException("شناسه دوره معتبر نیست", nameof(courseId));
    }

    private static void ValidateRating(decimal rating)
    {
        if (rating < 1 || rating > 5)
            throw new DomainException("امتیاز باید بین ۱ تا ۵ باشد", nameof(rating));

        if (rating % 0.5m != 0)
            throw new DomainException("امتیاز باید مضربی از نیم‌ستاره باشد", nameof(rating));
    }

    private static string? NormalizeComment(string? comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
            return null;

        var normalized = comment.Trim();

        if (normalized.Length > DomainConstants.MaxReviewCommentLength)
            throw new DomainException(
                $"متن نظر نمی‌تواند بیشتر از {DomainConstants.MaxReviewCommentLength} کاراکتر باشد",
                nameof(comment));

        return normalized;
    }
}