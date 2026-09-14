using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Chapters;

public sealed class GetAllChaptersQueryHandler
{
    private readonly IChapterRepository _chapterRepository;

    public GetAllChaptersQueryHandler(IChapterRepository chapterRepository)
    {
        _chapterRepository = chapterRepository;
    }

    public async Task<IEnumerable<ChapterDto>> Handle(
        GetAllChaptersQuery query,
        CancellationToken cancellationToken = default)
    {
        var chapters = await _chapterRepository.GetAllAsync();

        return chapters.Select(x => x.ToDto());
    }
}