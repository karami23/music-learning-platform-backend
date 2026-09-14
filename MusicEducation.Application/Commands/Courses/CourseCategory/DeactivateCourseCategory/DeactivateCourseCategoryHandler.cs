using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.CourseCategory.DeactivateCourseCategory;

public sealed class DeactivateCourseCategoryHandler
{
    private readonly ICourseCategoryRepository _courseCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateCourseCategoryHandler(
        ICourseCategoryRepository courseCategoryRepository,
        IUnitOfWork unitOfWork)
    {
        _courseCategoryRepository = courseCategoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        DeactivateCourseCategoryCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.CategoryId <= 0)
            throw new ValidationException(
                "شناسه دسته‌بندی معتبر نیست");

        var category = await _courseCategoryRepository
            .GetByIdAsync(command.CategoryId);

        if (category is null)
            throw new NotFoundException(
                "دسته‌بندی دوره پیدا نشد");

        category.Deactivate();

        await _courseCategoryRepository.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}