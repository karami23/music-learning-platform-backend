using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.Entities.Commerce;
using MusicEducation.Domain.Interfaces.Commerce;
using MusicEducation.Domain.Interfaces.Courses;

namespace MusicEducation.Application.Commands.Commerce.Order;

public sealed class AddItemToOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddItemToOrderCommandHandler(
        IOrderRepository orderRepository,
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        AddItemToOrderCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId <= 0)
            throw new ValidationException(
                "شناسه کاربر معتبر نیست");
        if (command.OrderId <= 0)
            throw new ValidationException(
                "شناسه سفارش معتبر نیست");

        if (command.CourseId <= 0)
            throw new ValidationException(
                "شناسه دوره معتبر نیست");

        var order = await _orderRepository
            .GetByIdAsync(command.OrderId);

        if (order is null)
            throw new NotFoundException(
                "سفارش موردنظر پیدا نشد");

        if (order.UserId != command.UserId)
            throw new UnauthorizedException(
                "شما دسترسی به این سفارش را ندارید");

        var course = await _courseRepository
            .GetByIdAsync(command.CourseId);

        if (course is null)
            throw new NotFoundException(
                "دوره موردنظر پیدا نشد");

        if (course.Price is null)
            throw new ValidationException(
                "دوره دارای قیمت معتبر نیست");

        var orderItem = OrderItem.Create(
            order.Id,
            course.Id,
            course.Title,
            course.Price);

        order.AddItem(orderItem);

        await _orderRepository.UpdateAsync(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}