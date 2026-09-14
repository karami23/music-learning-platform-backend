using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Mappings.Courses;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Queries.Courses.Categories;

public sealed class GetCourseCategoryByIdHandler
{
    private readonly ICourseCategoryRepository _courseCategoryRepository;

    public GetCourseCategoryByIdHandler(ICourseCategoryRepository courseCategoryRepository)
    {
        _courseCategoryRepository = courseCategoryRepository;
    }

    public async Task<CourseCategoryDto> HandleAsync(
        GetCourseCategoryByIdQuery query)
    {
        if (query.CategoryId <= 0)
            throw new ValidationException("شناسه دسته‌بندی معتبر نیست");

        var category = await _courseCategoryRepository.GetByIdAsync(query.CategoryId);

        if (category is null)
            throw new NotFoundException("دسته‌بندی دوره پیدا نشد");

        return category.ToDto();
    }
}