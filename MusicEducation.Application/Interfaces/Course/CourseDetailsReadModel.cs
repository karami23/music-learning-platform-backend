using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Interfaces.Courses;

public record CourseDetailsReadModel(
    int Id,
    string Title,
    string Slug,
    string Description,
    decimal? Price,
    string? Currency,
    CoursePricingType PricingType,
    CourseStatus Status,
    string? CoverImageUrl,
    int TeacherId,
    string TeacherName,
    string? TeacherBio,
    int CourseCategoryId,
    string CourseCategoryName
);