using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Courses;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.ValueObjects;

using CourseEntity =
    MusicEducation.Domain.Entities.Courses.Course;

namespace MusicEducation.Application.Commands.Courses.Course.CreateCourse;

public sealed class CreateCourseHandler
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICourseCategoryRepository _courseCategoryRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseHandler(
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

    public async Task<int> HandleAsync(
        CreateCourseCommand command,
        CancellationToken cancellationToken = default)
    {
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
            .ExistsBySlugAsync(normalizedSlug);

        if (slugExists)
            throw new ConflictException(
                "این Slug قبلاً برای یک دوره استفاده شده است");

        Money? price = command.PriceAmount.HasValue
            ? Money.Create(command.PriceAmount.Value)
            : null;

        var course = CourseEntity.Create(
            command.TeacherId,
            command.CourseCategoryId,
            command.Title,
            normalizedSlug,
            command.Description,
            command.PricingType,
            price,
            command.CoverImageUrl);

        await _courseRepository.AddAsync(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return course.Id;
    }
}