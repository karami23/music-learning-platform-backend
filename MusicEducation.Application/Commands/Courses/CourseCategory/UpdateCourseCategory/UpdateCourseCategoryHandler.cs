using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.CourseCategory.UpdateCourseCategory;

public sealed class UpdateCourseCategoryHandler
{
    private readonly ICourseCategoryRepository _courseCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseCategoryHandler(
        ICourseCategoryRepository courseCategoryRepository,
        IUnitOfWork unitOfWork)
    {
        _courseCategoryRepository = courseCategoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        UpdateCourseCategoryCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.CategoryId <= 0)
            throw new ValidationException(
                "شناسه دسته‌بندی معتبر نیست");

        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ValidationException(
                "نام دسته‌بندی دوره الزامی است");

        var category = await _courseCategoryRepository
            .GetByIdAsync(command.CategoryId);

        if (category is null)
            throw new NotFoundException(
                "دسته‌بندی دوره پیدا نشد");

        var exists = await _courseCategoryRepository
            .ExistsByNameAsync(
                command.Name.Trim(),
                command.CategoryId);

        if (exists)
            throw new ConflictException(
                "این نام دسته‌بندی قبلاً برای دسته‌بندی دیگری استفاده شده است");

        category.Update(
            command.Name,
            command.Description);

        await _courseCategoryRepository.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}