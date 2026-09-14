using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Mappings.Learning;
using LessonProgressEntity =
    MusicEducation.Domain.Entities.Learning.LessonProgress;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Commands.Learning.LessonProgress.CreateLessonProgress;

public sealed class CreateLessonProgressCommandHandler
{
    private readonly ILessonProgressRepository _lessonProgressRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLessonProgressCommandHandler(
        ILessonProgressRepository lessonProgressRepository,
        IUnitOfWork unitOfWork)
    {
        _lessonProgressRepository = lessonProgressRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LessonProgressDto> Handle(
        CreateLessonProgressCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (command.LessonId <= 0)
            throw new ValidationException(
                "شناسه جلسه معتبر نیست");

        var existingProgress =
            await _lessonProgressRepository
                .GetByUserIdAndLessonIdAsync(
                    command.UserId,
                    command.LessonId);

        if (existingProgress is not null)
            throw new ConflictException(
                "برای این کاربر و این جلسه، پیشرفت قبلاً ثبت شده است");

        var progress = LessonProgressEntity.Create(
            command.UserId,
            command.LessonId);

        var createdProgress =
            await _lessonProgressRepository
                .AddAsync(progress);

        await _unitOfWork
            .SaveChangesAsync(cancellationToken);

        return createdProgress.ToDto();
    }
}