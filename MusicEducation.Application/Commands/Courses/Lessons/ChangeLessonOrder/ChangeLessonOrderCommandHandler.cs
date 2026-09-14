using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.Lessons.ChangeLessonOrder;

public sealed class ChangeLessonOrderCommandHandler
{
    private readonly ILessonRepository _lessonRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeLessonOrderCommandHandler(
        ILessonRepository lessonRepository,
        IUnitOfWork unitOfWork)
    {
        _lessonRepository = lessonRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ChangeLessonOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.LessonId <= 0)
            throw new ValidationException(
                "شناسه جلسه معتبر نیست");

        if (command.Order <= 0)
            throw new ValidationException(
                "ترتیب جلسه معتبر نیست");

        var lesson = await _lessonRepository
            .GetByIdAsync(command.LessonId);

        if (lesson is null)
            throw new NotFoundException(
                "جلسه مورد نظر پیدا نشد");

        var existingLesson =
            await _lessonRepository
                .GetByChapterIdAndOrderAsync(
                    lesson.ChapterId,
                    command.Order);

        if (existingLesson is not null &&
            existingLesson.Id != lesson.Id)
        {
            throw new ConflictException(
                "این ترتیب قبلاً برای یک جلسه در این فصل استفاده شده است");
        }

        lesson.ChangeOrder(command.Order);

        await _lessonRepository.UpdateAsync(lesson);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}