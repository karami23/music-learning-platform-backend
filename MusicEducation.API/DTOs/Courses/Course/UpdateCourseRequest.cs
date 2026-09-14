using MusicEducation.Domain.Enums;

namespace MusicEducation.API.DTOs.Courses;

public sealed record UpdateCourseRequest(
    int TeacherId,
    int CourseCategoryId,
    string Title,
    string Slug,
    string Description,
    CoursePricingType PricingType,
    decimal? PriceAmount,
    string? CoverImageUrl
);