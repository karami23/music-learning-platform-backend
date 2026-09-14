using MusicEducation.Application.DTOs.Reviews;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Mappings.Reviews;
using MusicEducation.Domain.Interfaces.Reviews;

namespace MusicEducation.Application.Commands.Reviews.UpdateReview;

public sealed class UpdateReviewCommandHandler
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReviewDto> Handle(
        UpdateReviewCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (command.ReviewId <= 0)
            throw new ValidationException(
                "شناسه نظر معتبر نیست");

        if (command.Rating < 1 || command.Rating > 5)
            throw new ValidationException(
                "امتیاز باید بین 1 تا 5 باشد");

        var review = await _reviewRepository
            .GetByIdAsync(command.ReviewId);

        if (review is null)
            throw new NotFoundException(
                "نظر مورد نظر پیدا نشد");

        if (review.UserId != command.UserId)
            throw new UnauthorizedException(
                "شما دسترسی به این نظر را ندارید");

        review.Update(
            command.Rating,
            command.Comment);

        await _reviewRepository.UpdateAsync(review);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return review.ToDto();
    }
}