using MusicEducation.Application.DTOs.Reviews;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Mappings.Reviews;
using MusicEducation.Domain.Interfaces.Reviews;

namespace MusicEducation.Application.Commands.Reviews.ApproveReview;

public sealed class ApproveReviewCommandHandler
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ApproveReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReviewDto> Handle(
        ApproveReviewCommand command,
        CancellationToken cancellationToken = default)
    {
        var review = await _reviewRepository
            .GetByIdAsync(command.ReviewId);

        if (review is null)
            throw new NotFoundException("نظر مورد نظر پیدا نشد");

        review.Approve();

        await _reviewRepository.UpdateAsync(review);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return review.ToDto();
    }
}