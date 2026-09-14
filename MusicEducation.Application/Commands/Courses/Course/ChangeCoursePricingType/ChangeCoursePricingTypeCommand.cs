using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Commands.Courses.Course.ChangeCoursePricingType;

public sealed record ChangeCoursePricingTypeCommand(
    int CourseId,
    CoursePricingType PricingType,
    decimal? PriceAmount
);