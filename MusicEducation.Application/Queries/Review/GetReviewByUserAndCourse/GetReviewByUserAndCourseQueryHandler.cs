using MusicEducation.Application.DTOs.Reviews;
using MusicEducation.Application.Mappings.Reviews;
using MusicEducation.Domain.Interfaces.Reviews;

namespace MusicEducation.Application.Queries.Reviews.GetReviewByUserAndCourse;

public class GetReviewByUserAndCourseQueryHandler
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewByUserAndCourseQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<ReviewDto?> Handle(GetReviewByUserAndCourseQuery query)
    {
        var review = await _reviewRepository.GetByUserIdAndCourseIdAsync(query.UserId, query.CourseId);

        return review?.ToDto();
    }
}