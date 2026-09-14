using MusicEducation.Application.DTOs.Reviews;
using MusicEducation.Application.Mappings.Reviews;
using MusicEducation.Domain.Interfaces.Reviews;
using System.ComponentModel.DataAnnotations;

namespace MusicEducation.Application.Queries.Reviews.GetReviewById;

public class GetReviewByIdQueryHandler
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewByIdQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<ReviewDto?> Handle(GetReviewByIdQuery query)
    {
        var review = await _reviewRepository.GetByIdAsync(query.ReviewId);

        return review?.ToDto();
    }
}