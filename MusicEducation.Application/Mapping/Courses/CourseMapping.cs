using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Interfaces.Courses;

namespace MusicEducation.Application.Mappings.Courses;

public static class CourseMapping
{
    public static CourseListDto ToDto(
        this CourseListReadModel model)
    {
        return new CourseListDto(
            model.Id,
            model.Title,
            model.Slug,
            model.Price,
            model.Currency,
            model.PricingType,
            model.CoverImageUrl,
            model.TeacherId,
            model.TeacherName,
            model.CourseCategoryId,
            model.CourseCategoryName
        );
    }

    public static CourseDetailsDto ToDto(
        this CourseDetailsReadModel model)
    {
        return new CourseDetailsDto(
            model.Id,
            model.Title,
            model.Slug,
            model.Description,
            model.Price,
            model.Currency,
            model.PricingType,
            model.Status,
            model.CoverImageUrl,
            model.TeacherId,
            model.TeacherName,
            model.TeacherBio,
            model.CourseCategoryId,
            model.CourseCategoryName
        );
    }
}