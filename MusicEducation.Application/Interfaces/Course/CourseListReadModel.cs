using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Interfaces.Courses;

public record CourseListReadModel(
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