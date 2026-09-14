using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Exceptions;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Domain.Entities.Commerce;

public class OrderItem : BaseEntity
{
    public int OrderId { get; private set; }

    public int CourseId { get; private set; }

    public string CourseTitle { get; private set; } = null!;

    public Money UnitPrice { get; private set; } = null!;

    private OrderItem() { }

    public static OrderItem Create(
        int orderId,
        int courseId,
        string courseTitle,
        Money unitPrice)
    {
        ValidateOrderId(orderId);
        ValidateCourseId(courseId);

        var normalizedCourseTitle = NormalizeCourseTitle(courseTitle);

        ArgumentNullException.ThrowIfNull(unitPrice);

        return new OrderItem
        {
            OrderId = orderId,
            CourseId = courseId,
            CourseTitle = normalizedCourseTitle,
            UnitPrice = unitPrice
        };
    }

    private static void ValidateOrderId(int orderId)
    {
        if (orderId <= 0)
            throw new DomainException("شناسه سفارش معتبر نیست", nameof(orderId));
    }

    private static void ValidateCourseId(int courseId)
    {
        if (courseId <= 0)
            throw new DomainException("شناسه دوره معتبر نیست", nameof(courseId));
    }

    private static string NormalizeCourseTitle(string courseTitle)
    {
        if (string.IsNullOrWhiteSpace(courseTitle))
            throw new DomainException("عنوان دوره الزامی است", nameof(courseTitle));

        var normalized = courseTitle.Trim();

        if (normalized.Length > DomainConstants.MaxOrderItemCourseTitleLength)
            throw new DomainException("عنوان دوره نمی‌تواند بیشتر از 200 کاراکتر باشد", nameof(courseTitle));

        return normalized;
    }
}