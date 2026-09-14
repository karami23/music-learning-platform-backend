using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.LessonMedia.UpdateLessonMedia;

public sealed class UpdateLessonMediaCommandHandler
{
    private readonly ILessonMediaRepository _lessonMediaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLessonMediaCommandHandler(
        ILessonMediaRepository lessonMediaRepository,
        IUnitOfWork unitOfWork)
    {
        _lessonMediaRepository = lessonMediaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UpdateLessonMediaCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.MediaId <= 0)
            throw new ValidationException(
                "شناسه محتوای آموزشی معتبر نیست");

        if (command.Order <= 0)
            throw new ValidationException(
                "ترتیب محتوا معتبر نیست");

        var media = await _lessonMediaRepository
            .GetByIdAsync(command.MediaId);

        if (media is null)
            throw new NotFoundException(
                "محتوای آموزشی موردنظر پیدا نشد");

        var existingMedia =
            await _lessonMediaRepository
                .GetByLessonIdAndOrderAsync(
                    media.LessonId,
                    command.Order);

        if (existingMedia is not null &&
            existingMedia.Id != media.Id)
        {
            throw new ConflictException(
                "این ترتیب قبلاً برای یک محتوای دیگر در این جلسه استفاده شده است");
        }

        media.Update(
            command.Title,
            command.FileName,
            command.StorageKey,
            command.Order,
            command.Duration);

        await _lessonMediaRepository.UpdateAsync(media);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}