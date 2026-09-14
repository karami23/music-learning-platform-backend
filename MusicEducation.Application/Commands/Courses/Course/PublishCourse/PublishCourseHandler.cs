using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.Course.PublishCourse;

public sealed class PublishCourseHandler
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PublishCourseHandler(
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        PublishCourseCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.CourseId <= 0)
            throw new ValidationException(
                "شناسه دوره معتبر نیست");

        var course = await _courseRepository
            .GetByIdAsync(command.CourseId);

        if (course is null)
            throw new NotFoundException(
                "دوره پیدا نشد");

        course.Publish();

        await _courseRepository.UpdateAsync(course);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}
