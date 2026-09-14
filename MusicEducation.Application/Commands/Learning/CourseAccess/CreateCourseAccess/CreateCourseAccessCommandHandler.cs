using MusicEducation.Application.DTOs.Learning;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Mappings.Learning;
using CourseAccessEntity = MusicEducation.Domain.Entities.Learning.CourseAccess;
using MusicEducation.Domain.Interfaces.Learning;

namespace MusicEducation.Application.Commands.Learning.CourseAccess.CreateCourseAccess;

public sealed class CreateCourseAccessCommandHandler
{
    private readonly ICourseAccessRepository _courseAccessRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseAccessCommandHandler(
        ICourseAccessRepository courseAccessRepository,
        IUnitOfWork unitOfWork)
    {
        _courseAccessRepository = courseAccessRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CourseAccessDto> Handle(
        CreateCourseAccessCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");

        if (command.CourseId <= 0)
            throw new ValidationException(
                "شناسه دوره معتبر نیست");

        var hasAccess = await _courseAccessRepository
            .HasAccessAsync(
                command.UserId,
                command.CourseId);

        if (hasAccess)
            throw new ConflictException(
                "این کاربر به این دوره دسترسی دارد");

        var courseAccess = CourseAccessEntity.Create(
            command.UserId,
            command.CourseId);

        var createdCourseAccess =
            await _courseAccessRepository.AddAsync(courseAccess);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return createdCourseAccess.ToDto();
    }
}