using MusicEducation.Application.DTOs.Courses;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces.Courses;
using MusicEducation.Application.Mappings.Courses;

namespace MusicEducation.Application.Queries.Courses;

public sealed class GetCoursesHandler
{
    private readonly ICourseReadRepository _courseReadRepository;

    public GetCoursesHandler(ICourseReadRepository courseReadRepository)
    {
        _courseReadRepository = courseReadRepository;
    }

    public async Task<IEnumerable<CourseListDto>> HandleAsync(GetCoursesQuery query)
    {
        if (query.Page <= 0)
            throw new ValidationException("شماره صفحه باید بزرگ‌تر از صفر باشد");

        if (query.PageSize <= 0)
            throw new ValidationException("تعداد آیتم‌های صفحه باید بزرگ‌تر از صفر باشد");

        if (query.PageSize > 100)
            throw new ValidationException("تعداد آیتم‌های صفحه نمی‌تواند بیشتر از 100 باشد");

        var courses = await _courseReadRepository.GetPagedAsync(
            query.Search,
            query.TeacherId,
            query.CourseCategoryId,
            query.Page,
            query.PageSize);

        return courses
            .Select(x => x.ToDto())
            .ToList();
    }
}