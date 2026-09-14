using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.LessonMedia.ChangeLessonMediaOrder;

public sealed class ChangeLessonMediaOrderCommandHandler
{
    private readonly ILessonMediaRepository _lessonMediaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeLessonMediaOrderCommandHandler(
        ILessonMediaRepository lessonMediaRepository,
        IUnitOfWork unitOfWork)
    {
        _lessonMediaRepository = lessonMediaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ChangeLessonMediaOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.LessonMediaId <= 0)
            throw new ValidationException(
                "شناسه محتوای آموزشی معتبر نیست");

        if (command.Order <= 0)
            throw new ValidationException(
                "ترتیب محتوا معتبر نیست");

        var media = await _lessonMediaRepository
            .GetByIdAsync(command.LessonMediaId);

        if (media is null)
            throw new NotFoundException(
                "محتوای مورد نظر پیدا نشد");

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

        media.ChangeOrder(command.Order);

        await _lessonMediaRepository.UpdateAsync(media);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}