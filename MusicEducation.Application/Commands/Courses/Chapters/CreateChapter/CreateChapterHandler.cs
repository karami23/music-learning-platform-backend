using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.Chapters.CreateChapter;

public sealed class CreateChapterHandler
{
    private readonly IChapterRepository _chapterRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateChapterHandler(
        IChapterRepository chapterRepository,
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _chapterRepository = chapterRepository;
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> HandleAsync(
        CreateChapterCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.CourseId <= 0)
            throw new ValidationException("شناسه دوره معتبر نیست");

        if (command.Order <= 0)
            throw new ValidationException("ترتیب فصل معتبر نیست");

        var course = await _courseRepository
            .GetByIdAsync(command.CourseId);

        if (course is null)
            throw new NotFoundException("دوره پیدا نشد");

        var existingChapter =
            await _chapterRepository.GetByCourseIdAndOrderAsync(
                command.CourseId,
                command.Order);

        if (existingChapter is not null)
            throw new ConflictException(
                "این ترتیب قبلاً برای فصلی از این دوره استفاده شده است");

        var chapter = Chapter.Create(
            command.CourseId,
            command.Title,
            command.Description,
            command.Order);

        await _chapterRepository.AddAsync(chapter);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return chapter.Id;
    }
}