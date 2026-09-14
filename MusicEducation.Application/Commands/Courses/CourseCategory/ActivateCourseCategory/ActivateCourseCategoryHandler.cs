using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.CourseCategory.ActivateCourseCategory;

public sealed class ActivateCourseCategoryHandler
{
    private readonly ICourseCategoryRepository _courseCategoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateCourseCategoryHandler(
        ICourseCategoryRepository courseCategoryRepository,
        IUnitOfWork unitOfWork)
    {
        _courseCategoryRepository = courseCategoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        ActivateCourseCategoryCommand command,
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

        category.Activate();

        await _courseCategoryRepository.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}