using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Courses.Course.ArchiveCourse;

public sealed class ArchiveCourseHandler
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ArchiveCourseHandler(
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ArchiveCourseCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.CourseId <= 0)
            throw new ValidationException("شناسه دوره معتبر نیست");

        var course = await _courseRepository
            .GetByIdAsync(command.CourseId);

        if (course is null)
            throw new NotFoundException("دوره پیدا نشد");

        course.Archive();

        await _courseRepository.UpdateAsync(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}