using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Courses;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Commands.Courses.Course.UpdateMyCourse;

public sealed class UpdateMyCourseHandler
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICourseCategoryRepository _courseCategoryRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMyCourseHandler(
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
        UpdateMyCourseCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (command.CourseId <= 0)
            throw new ValidationException(
                "شناسه دوره معتبر نیست");

        if (command.CourseCategoryId <= 0)
            throw new ValidationException(
                "شناسه دسته‌بندی دوره معتبر نیست");

        if (string.IsNullOrWhiteSpace(command.Slug))
            throw new ValidationException(
                "Slug دوره الزامی است");

        if (command.PriceAmount.HasValue &&
            command.PriceAmount.Value < 0)
        {
            throw new ValidationException(
                "مبلغ دوره نمی‌تواند منفی باشد");
        }

        var teacher = await _teacherRepository
            .GetByUserIdAsync(command.UserId);

        if (teacher is null)
            throw new NotFoundException(
                "مدرس پیدا نشد");

        if (teacher.Status != TeacherStatus.Active)
            throw new ConflictException(
                "مدرس فعال نیست");

        var course = await _courseRepository
            .GetByIdAsync(command.CourseId);

        if (course is null)
            throw new NotFoundException(
                "دوره پیدا نشد");

        if (course.TeacherId != teacher.Id)
            throw new NotFoundException(
                "دوره پیدا نشد");

        if (course.Status != CourseStatus.Draft &&
            course.Status != CourseStatus.PendingReview)
        {
            throw new ConflictException(
                "این دوره در وضعیت فعلی قابل ویرایش نیست");
        }

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
                "این Slug قبلاً برای یک دوره استفاده شده است");

        Money? price = command.PriceAmount.HasValue
            ? Money.Create(command.PriceAmount.Value)
            : null;

        course.Update(
            teacher.Id,
            command.CourseCategoryId,
            command.Title,
            normalizedSlug,
            command.Description,
            command.PricingType,
            price,
            command.CoverImageUrl);

        await _courseRepository.UpdateAsync(course);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}