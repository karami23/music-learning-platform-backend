using MusicEducation.Application.DTOs.Reviews;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Mappings.Reviews;
using MusicEducation.Domain.Entities.Reviews;
using MusicEducation.Domain.Interfaces.Reviews;

namespace MusicEducation.Application.Commands.Reviews.CreateReview;

public sealed class CreateReviewCommandHandler
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ReviewDto> Handle(
        CreateReviewCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (command.CourseId <= 0)
            throw new ValidationException(
                "شناسه دوره معتبر نیست");

        if (command.Rating < 1 || command.Rating > 5)
            throw new ValidationException(
                "امتیاز باید بین 1 تا 5 باشد");

        var hasReviewed = await _reviewRepository
            .HasReviewedAsync(
                command.UserId,
                command.CourseId);

        if (hasReviewed)
            throw new ConflictException(
                "این کاربر قبلاً برای این دوره نظر ثبت کرده است");

        var review = Review.Create(
            command.UserId,
            command.CourseId,
            command.Rating,
            command.Comment);

        var createdReview =
            await _reviewRepository.AddAsync(review);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return createdReview.ToDto();
    }
}