using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.Commands.Courses.Course.ChangeCoursePrice;

public sealed record ChangeCoursePriceCommand(
    int CourseId,
    decimal? PriceAmount
);