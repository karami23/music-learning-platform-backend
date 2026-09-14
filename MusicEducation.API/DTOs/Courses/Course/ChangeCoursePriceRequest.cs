namespace MusicEducation.API.DTOs.Courses;

public sealed record ChangeCoursePriceRequest(
    decimal? PriceAmount
);