using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Interfaces.Courses;
using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Commands.Courses.Course.ChangeCoursePrice;

public sealed class ChangeCoursePriceHandler
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeCoursePriceHandler(
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        ChangeCoursePriceCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.CourseId <= 0)
            throw new ValidationException("شناسه دوره معتبر نیست");

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

        Money? price = command.PriceAmount.HasValue
            ? Money.Create(command.PriceAmount.Value)
            : null;

        course.ChangePrice(price);

        await _courseRepository.UpdateAsync(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}