using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Chapters;

public sealed class GetChaptersByCourseIdQueryHandler
{
    private readonly IChapterRepository _chapterRepository;

    public GetChaptersByCourseIdQueryHandler(IChapterRepository chapterRepository)
    {
        _chapterRepository = chapterRepository;
    }

    public async Task<IEnumerable<ChapterDto>> Handle(
        GetChaptersByCourseIdQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.CourseId <= 0)
            throw new ValidationException("شناسه دوره معتبر نیست");

        var chapters = await _chapterRepository.GetByCourseIdAsync(query.CourseId);

        return chapters
            .OrderBy(x => x.Order)
            .Select(x => x.ToDto());
    }
}