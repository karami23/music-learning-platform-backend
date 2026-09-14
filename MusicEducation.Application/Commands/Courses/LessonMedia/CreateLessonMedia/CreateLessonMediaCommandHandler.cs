using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using LessonMediaEntity =
    MusicEducation.Domain.Entities.Courses.LessonMedia;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.LessonMedia.CreateLessonMedia;

public sealed class CreateLessonMediaCommandHandler
{
    private readonly ILessonMediaRepository _lessonMediaRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLessonMediaCommandHandler(
        ILessonMediaRepository lessonMediaRepository,
        ILessonRepository lessonRepository,
        IUnitOfWork unitOfWork)
    {
        _lessonMediaRepository = lessonMediaRepository;
        _lessonRepository = lessonRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreateLessonMediaCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.LessonId <= 0)
            throw new ValidationException(
                "شناسه جلسه معتبر نیست");

        if (command.Order <= 0)
            throw new ValidationException(
                "ترتیب محتوا معتبر نیست");

        var lesson = await _lessonRepository
            .GetByIdAsync(command.LessonId);

        if (lesson is null)
            throw new NotFoundException(
                "جلسه مورد نظر پیدا نشد");

        var existingMedia =
            await _lessonMediaRepository
                .GetByLessonIdAndOrderAsync(
                    command.LessonId,
                    command.Order);

        if (existingMedia is not null)
            throw new ConflictException(
                "این ترتیب قبلاً برای یک محتوای دیگر در این جلسه استفاده شده است");

        var media = LessonMediaEntity.Create(
            command.LessonId,
            command.MediaType,
            command.Title,
            command.FileName,
            command.StorageKey,
            command.Order,
            command.Duration);

        await _lessonMediaRepository.AddAsync(media);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return media.Id;
    }
}