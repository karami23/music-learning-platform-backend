using MusicEducation.Application.Exceptions;
using MusicEducation.Domain.Interfaces.Courses;
using MusicEducation.Domain.Interfaces.Identity;
using MusicEducation.Application.Interfaces;

namespace MusicEducation.Application.Commands.Courses.SubmitCourseForReview;

public sealed class SubmitCourseForReviewCommandHandler
{
    private readonly ICourseRepository _courseRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitCourseForReviewCommandHandler(
        ICourseRepository courseRepository,
        ITeacherRepository teacherRepository,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _teacherRepository = teacherRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        SubmitCourseForReviewCommand command)
    {
        if (command.UserId <= 0)
            throw new ValidationException("شناسه کاربر معتبر نیست");

        if (command.CourseId <= 0)
            throw new ValidationException("شناسه دوره معتبر نیست");

        var teacher = await _teacherRepository
            .GetByUserIdAsync(command.UserId);

        if (teacher is null)
            throw new NotFoundException("مدرس پیدا نشد");

        if (teacher.Status != Domain.Enums.TeacherStatus.Active)
            throw new ConflictException("مدرس فعال نیست");

        var course = await _courseRepository
            .GetByIdAsync(command.CourseId);

        if (course is null)
            throw new NotFoundException("دوره پیدا نشد");

        if (course.TeacherId != teacher.Id)
            throw new NotFoundException("دوره پیدا نشد");

        course.SubmitForReview();

        await _courseRepository.UpdateAsync(course);
        await _unitOfWork.SaveChangesAsync();
    }
}