using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Mappings.Learning;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Commands.Learning.CourseAccess.RestoreCourseAccess;

public sealed class RestoreCourseAccessCommandHandler
{
    private readonly ICourseAccessRepository _courseAccessRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RestoreCourseAccessCommandHandler(
        ICourseAccessRepository courseAccessRepository,
        IUnitOfWork unitOfWork)
    {
        _courseAccessRepository = courseAccessRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CourseAccessDto> Handle(
        RestoreCourseAccessCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.CourseAccessId <= 0)
            throw new ValidationException(
                "شناسه دسترسی دوره معتبر نیست");

        var courseAccess = await _courseAccessRepository
            .GetByIdAsync(command.CourseAccessId);

        if (courseAccess is null)
            throw new NotFoundException(
                "دسترسی دوره موردنظر پیدا نشد");

        courseAccess.Restore();

        await _courseAccessRepository
            .UpdateAsync(courseAccess);

        await _unitOfWork
            .SaveChangesAsync(cancellationToken);

        return courseAccess.ToDto();
    }
}