using MusicEducation.Application.DTOs.Reviews;
using MusicEducation.Application.Mappings.Reviews;
using MusicEducation.Domain.Interfaces.Reviews;
using System.ComponentModel.DataAnnotations;

namespace MusicEducation.Application.Queries.Reviews.GetApprovedReviewsByCourseId;

public class GetApprovedReviewsByCourseIdQueryHandler
{
    private readonly IReviewRepository _reviewRepository;

    public GetApprovedReviewsByCourseIdQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<IEnumerable<ReviewDto>> Handle(GetApprovedReviewsByCourseIdQuery query)
    {
        if (query.CourseId<= 0)
            throw new ValidationException(
                "شناسه دوره معتبر نیست");
        var reviews = await _reviewRepository.GetApprovedByCourseIdAsync(query.CourseId);

        return reviews.Select(x => x.ToDto());
    }
}