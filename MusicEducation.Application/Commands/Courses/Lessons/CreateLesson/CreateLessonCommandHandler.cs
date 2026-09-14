using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.Lessons.CreateLesson;

public sealed class CreateLessonCommandHandler
{
    private readonly ILessonRepository _lessonRepository;
    private readonly IChapterRepository _chapterRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateLessonCommandHandler(
        ILessonRepository lessonRepository,
        IChapterRepository chapterRepository,
        IUnitOfWork unitOfWork)
    {
        _lessonRepository = lessonRepository;
        _chapterRepository = chapterRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(
        CreateLessonCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.ChapterId <= 0)
            throw new ValidationException(
                "شناسه فصل معتبر نیست");

        if (command.Order <= 0)
            throw new ValidationException(
                "ترتیب جلسه معتبر نیست");

        var chapter = await _chapterRepository
            .GetByIdAsync(command.ChapterId);

        if (chapter is null)
            throw new NotFoundException(
                "فصل مورد نظر پیدا نشد");

        var existingLesson =
            await _lessonRepository
                .GetByChapterIdAndOrderAsync(
                    command.ChapterId,
                    command.Order);

        if (existingLesson is not null)
            throw new ConflictException(
                "این ترتیب قبلاً برای یک جلسه در این فصل استفاده شده است");

        var lesson = Lesson.Create(
            command.ChapterId,
            command.Title,
            command.Description,
            command.Order);

        await _lessonRepository.AddAsync(lesson);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return lesson.Id;
    }
}