using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Commands.Learning.LessonProgress.UpdateLessonProgress;

public sealed class UpdateLessonProgressCommandHandler
{
    private readonly ILessonProgressRepository _lessonProgressRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLessonProgressCommandHandler(
        ILessonProgressRepository lessonProgressRepository,
        IUnitOfWork unitOfWork)
    {
        _lessonProgressRepository = lessonProgressRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LessonProgressDto> Handle(
        UpdateLessonProgressCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (command.LessonProgressId <= 0)
            throw new ValidationException(
                "شناسه پیشرفت معتبر نیست");

        if (command.MediaId <= 0)
            throw new ValidationException(
                "شناسه رسانه معتبر نیست");

        if (command.Position < TimeSpan.Zero)
            throw new ValidationException(
                "موقعیت پخش نمی‌تواند منفی باشد");

        if (command.ProgressPercentage < 0 ||
            command.ProgressPercentage > 100)
        {
            throw new ValidationException(
                "درصد پیشرفت باید بین 0 تا 100 باشد");
        }

        var progress = await _lessonProgressRepository
            .GetByIdAsync(command.LessonProgressId);

        if (progress is null)
            throw new NotFoundException(
                "رکورد پیشرفت موردنظر پیدا نشد");

        if (progress.UserId != command.UserId)
            throw new UnauthorizedException(
                "شما دسترسی به این رکورد پیشرفت را ندارید");

        progress.UpdateProgress(
            command.MediaId,
            command.Position,
            command.ProgressPercentage);

        await _lessonProgressRepository
            .UpdateAsync(progress);

        await _unitOfWork
            .SaveChangesAsync(cancellationToken);

        return progress.ToDto();
    }
}
