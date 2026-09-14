using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Commands.Learning.LessonProgress.MarkLessonAsCompleted;

public sealed class MarkLessonAsCompletedCommandHandler
{
    private readonly ILessonProgressRepository _lessonProgressRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkLessonAsCompletedCommandHandler(
        ILessonProgressRepository lessonProgressRepository,
        IUnitOfWork unitOfWork)
    {
        _lessonProgressRepository = lessonProgressRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LessonProgressDto> Handle(
        MarkLessonAsCompletedCommand command,
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

        progress.MarkAsCompleted();

        await _lessonProgressRepository
            .UpdateAsync(progress);

        await _unitOfWork
            .SaveChangesAsync(cancellationToken);

        return progress.ToDto();
    }
}