using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.RejectCourse;

public sealed class RejectCourseCommandHandler
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectCourseCommandHandler(
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        RejectCourseCommand command)
    {
        if (command.CourseId <= 0)
            throw new ValidationException(
                "شناسه دوره معتبر نیست");

        var course = await _courseRepository
            .GetByIdAsync(command.CourseId);

        if (course is null)
            throw new NotFoundException(
                "دوره پیدا نشد");

        course.Reject();

        await _courseRepository.UpdateAsync(course);

        await _unitOfWork.SaveChangesAsync();
    }
}