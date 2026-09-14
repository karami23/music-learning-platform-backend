using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using CourseCategoryEntity =
    MusicEducation.Domain.Entities.Courses.CourseCategory;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.CourseCategory.CreateCourseCategory;

public sealed class CreateCourseCategoryHandler
{
    private readonly ICourseCategoryRepository _courseCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseCategoryHandler(
        ICourseCategoryRepository courseCategoryRepository,
        IUnitOfWork unitOfWork)
    {
        _courseCategoryRepository = courseCategoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> HandleAsync(
        CreateCourseCategoryCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ValidationException(
                "نام دسته‌بندی دوره الزامی است");

        var exists = await _courseCategoryRepository
            .ExistsByNameAsync(command.Name.Trim());

        if (exists)
            throw new ConflictException(
                "این نام دسته‌بندی قبلاً استفاده شده است");

        var category = CourseCategoryEntity.Create(
            command.Name,
            command.Description);

        await _courseCategoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}