using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Commands.Learning.LessonProgress.ResetLessonProgress;

public sealed class ResetLessonProgressCommandHandler
{
    private readonly ILessonProgressRepository _lessonProgressRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ResetLessonProgressCommandHandler(
        ILessonProgressRepository lessonProgressRepository,
        IUnitOfWork unitOfWork)
    {
        _lessonProgressRepository = lessonProgressRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LessonProgressDto> Handle(
        ResetLessonProgressCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (command.LessonProgressId <= 0)
            throw new ValidationException(
                "شناسه پیشرفت معتبر نیست");

        var progress = await _lessonProgressRepository
            .GetByIdAsync(command.LessonProgressId);

        if (progress is null)
            throw new NotFoundException(
                "رکورد پیشرفت موردنظر پیدا نشد");

        if (progress.UserId != command.UserId)
            throw new UnauthorizedException(
                "شما دسترسی به این رکورد پیشرفت را ندارید");

        progress.Reset();

        await _lessonProgressRepository
            .UpdateAsync(progress);

        await _unitOfWork
            .SaveChangesAsync(cancellationToken);

        return progress.ToDto();
    }
}