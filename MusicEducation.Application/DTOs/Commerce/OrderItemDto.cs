namespace MusicEducation.Application.DTOs.Commerce;

public record OrderItemDto(
    int Id,
    int OrderId,
    int CourseId,
    string CourseTitle,
    decimal UnitPrice,
    string Currency
);