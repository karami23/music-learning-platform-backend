using MusicEducation.Application.DTOs.Reviews;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Reviews;
using MusicEducation.Domain.Interfaces.Reviews;

namespace MusicEducation.Application.Queries.Reviews.GetReviewsByUserId;

public class GetReviewsByUserIdQueryHandler
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewsByUserIdQueryHandler(
        IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<IEnumerable<ReviewDto>> Handle(
        GetReviewsByUserIdQuery query)
    {
        if (query.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        var reviews = await _reviewRepository
            .GetByUserIdAsync(query.UserId);

        return reviews.Select(x => x.ToDto());
    }
}