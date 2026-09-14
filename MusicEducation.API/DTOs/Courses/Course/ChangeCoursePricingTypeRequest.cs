using MusicEducation.Domain.Enums;

namespace MusicEducation.API.DTOs.Courses;

public sealed record ChangeCoursePricingTypeRequest(
    CoursePricingType PricingType,
    decimal? PriceAmount
);