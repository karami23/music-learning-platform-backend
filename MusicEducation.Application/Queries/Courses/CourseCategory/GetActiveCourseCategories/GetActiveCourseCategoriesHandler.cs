using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Categories;

public sealed class GetActiveCourseCategoriesHandler
{
    private readonly ICourseCategoryRepository _courseCategoryRepository;

    public GetActiveCourseCategoriesHandler(ICourseCategoryRepository courseCategoryRepository)
    {
        _courseCategoryRepository = courseCategoryRepository;
    }

    public async Task<IEnumerable<CourseCategoryDto>> HandleAsync(GetActiveCourseCategoriesQuery query)
    {
        var categories = await _courseCategoryRepository.GetActiveAsync();

        return categories
            .Select(x => x.ToDto())
            .ToList();
    }
}