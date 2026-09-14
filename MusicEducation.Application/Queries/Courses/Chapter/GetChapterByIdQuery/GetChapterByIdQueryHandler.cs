using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Chapters;

public sealed class GetChapterByIdQueryHandler
{
    private readonly IChapterRepository _chapterRepository;

    public GetChapterByIdQueryHandler(IChapterRepository chapterRepository)
    {
        _chapterRepository = chapterRepository;
    }

    public async Task<ChapterDto> Handle(
        GetChapterByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.ChapterId <= 0)
            throw new ValidationException("شناسه فصل معتبر نیست");

        var chapter = await _chapterRepository.GetByIdAsync(query.ChapterId);

        if (chapter is null)
            throw new NotFoundException("فصل مورد نظر پیدا نشد");

        return chapter.ToDto();
    }
}