using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.Chapters.ChangeChapterOrder;

public sealed class ChangeChapterOrderHandler
{
    private readonly IChapterRepository _chapterRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeChapterOrderHandler(
        IChapterRepository chapterRepository,
        IUnitOfWork unitOfWork)
    {
        _chapterRepository = chapterRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        ChangeChapterOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.ChapterId <= 0)
            throw new ValidationException("شناسه فصل معتبر نیست");

        if (command.Order <= 0)
            throw new ValidationException("ترتیب فصل معتبر نیست");

        var chapter = await _chapterRepository
            .GetByIdAsync(command.ChapterId);

        if (chapter is null)
            throw new NotFoundException("فصل پیدا نشد");

        var existingChapter =
            await _chapterRepository.GetByCourseIdAndOrderAsync(
                chapter.CourseId,
                command.Order);

        if (existingChapter is not null &&
            existingChapter.Id != chapter.Id)
        {
            throw new ConflictException(
                "این ترتیب قبلاً برای فصل دیگری از این دوره استفاده شده است");
        }

        chapter.ChangeOrder(command.Order);

        await _chapterRepository.UpdateAsync(chapter);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}