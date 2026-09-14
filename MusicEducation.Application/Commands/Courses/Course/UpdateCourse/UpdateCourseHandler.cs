using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Courses;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Commands.Courses.Course.UpdateCourse;

public sealed class UpdateCourseHandler
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICourseCategoryRepository _courseCategoryRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseHandler(
        ICourseRepository courseRepository,
        ICourseCategoryRepository courseCategoryRepository,
        ITeacherRepository teacherRepository,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _courseCategoryRepository = courseCategoryRepository;
        _teacherRepository = teacherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        UpdateCourseCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.CourseId <= 0)
            throw new ValidationException("شناسه دوره معتبر نیست");

        if (command.TeacherId <= 0)
            throw new ValidationException("شناسه مدرس معتبر نیست");

        if (command.CourseCategoryId <= 0)
            throw new ValidationException(
                "شناسه دسته‌بندی دوره معتبر نیست");

        if (string.IsNullOrWhiteSpace(command.Slug))
            throw new ValidationException("Slug دوره الزامی است");

        if (command.PriceAmount.HasValue &&
            command.PriceAmount.Value < 0)
        {
            throw new ValidationException(
                "مبلغ دوره نمی‌تواند منفی باشد");
        }

        var course = await _courseRepository
            .GetByIdAsync(command.CourseId);

        if (course is null)
            throw new NotFoundException("دوره پیدا نشد");

        var teacher = await _teacherRepository
            .GetByIdAsync(command.TeacherId);

        if (teacher is null)
            throw new NotFoundException("مدرس پیدا نشد");

        if (teacher.Status != TeacherStatus.Active)
            throw new ConflictException("مدرس فعال نیست");

        var category = await _courseCategoryRepository
            .GetByIdAsync(command.CourseCategoryId);

        if (category is null)
            throw new NotFoundException(
                "دسته‌بندی دوره پیدا نشد");

        var normalizedSlug = command.Slug
            .Trim()
            .ToLowerInvariant();

        var slugExists = await _courseRepository
            .ExistsBySlugAsync(
                normalizedSlug,
                command.CourseId);

        if (slugExists)
            throw new ConflictException(
                "این Slug قبلاً برای یک دوره دیگر استفاده شده است");

        Money? price = command.PriceAmount.HasValue
            ? Money.Create(command.PriceAmount.Value)
            : null;

        course.Update(
            command.TeacherId,
            command.CourseCategoryId,
            command.Title,
            normalizedSlug,
            command.Description,
            command.PricingType,
            price,
            command.CoverImageUrl);

        await _courseRepository.UpdateAsync(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}