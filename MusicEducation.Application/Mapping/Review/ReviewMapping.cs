using MusicEducation.Application.DTOs.Reviews;
using MusicEducation.Domain.Entities.Reviews;

namespace MusicEducation.Application.Mappings.Reviews;

public static class ReviewMapping
{
    public static ReviewDto ToDto(this Review review)
    {
        return new ReviewDto(
            review.Id,
            review.UserId,
            review.CourseId,
            review.Rating,
            review.Comment,
            review.Status,
            review.ReviewedAt
        );
    }
}