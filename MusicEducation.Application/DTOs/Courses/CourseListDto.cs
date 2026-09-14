using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.DTOs.Courses;

public record CourseListDto(
    int Id,
    string Title,
    string Slug,
    decimal? Price,
    string? Currency,
    CoursePricingType PricingType,
    string? CoverImageUrl,
    int TeacherId,
    string TeacherName,
    int CourseCategoryId,
    string CourseCategoryName
);