using MusicEducation.Application.DTOs.Reviews;
using MusicEducation.Application.Mappings.Reviews;
using MusicEducation.Domain.Interfaces.Reviews;

namespace MusicEducation.Application.Queries.Reviews.GetReviewsByCourseId;

public class GetReviewsByCourseIdQueryHandler
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewsByCourseIdQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<IEnumerable<ReviewDto>> Handle(GetReviewsByCourseIdQuery query)
    {
        var reviews = await _reviewRepository.GetByCourseIdAsync(query.CourseId);

        return reviews.Select(x => x.ToDto());
    }
}