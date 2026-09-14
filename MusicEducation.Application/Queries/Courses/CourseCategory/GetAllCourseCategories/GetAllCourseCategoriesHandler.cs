using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Categories;

public sealed class GetAllCourseCategoriesHandler
{
    private readonly ICourseCategoryRepository _courseCategoryRepository;

    public GetAllCourseCategoriesHandler(ICourseCategoryRepository courseCategoryRepository)
    {
        _courseCategoryRepository = courseCategoryRepository;
    }

    public async Task<IEnumerable<CourseCategoryDto>> HandleAsync(GetAllCourseCategoriesQuery query)
    {
        var categories = await _courseCategoryRepository.GetAllAsync();

        return categories
            .Select(x => x.ToDto())
            .ToList();
    }
}