using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.API.DTOs.Commerce.Order;
using MusicEducation.Application.Commands.Commerce.Order;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Queries.Commerce.Order;
using MusicEducation.Domain.Enums;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class OrderController : ControllerBase
{
    private readonly AddItemToOrderCommandHandler _addItemHandler;
    private readonly ApplyDiscountToOrderCommandHandler _applyDiscountHandler;
    private readonly CancelOrderCommandHandler _cancelHandler;
    private readonly CreateOrderCommandHandler _createHandler;
    private readonly MarkOrderAsFailedCommandHandler _markAsFailedHandler;
    private readonly MarkOrderAsPaidCommandHandler _markAsPaidHandler;
    private readonly RemoveItemFromOrderCommandHandler _removeItemHandler;

    private readonly GetOrderByIdQueryHandler _getByIdHandler;
    private readonly GetOrderByOrderNumberQueryHandler _getByOrderNumberHandler;
    private readonly GetOrdersByUserIdQueryHandler _getByUserIdHandler;
    private readonly GetOrdersByUserIdAndStatusQueryHandler _getByUserIdAndStatusHandler;

    public OrderController(
        AddItemToOrderCommandHandler addItemHandler,
        ApplyDiscountToOrderCommandHandler applyDiscountHandler,
        CancelOrderCommandHandler cancelHandler,
        CreateOrderCommandHandler createHandler,
        MarkOrderAsFailedCommandHandler markAsFailedHandler,
        MarkOrderAsPaidCommandHandler markAsPaidHandler,
        RemoveItemFromOrderCommandHandler removeItemHandler,
        GetOrderByIdQueryHandler getByIdHandler,
        GetOrderByOrderNumberQueryHandler getByOrderNumberHandler,
        GetOrdersByUserIdQueryHandler getByUserIdHandler,
        GetOrdersByUserIdAndStatusQueryHandler getByUserIdAndStatusHandler)
    {
        _addItemHandler = addItemHandler;
        _applyDiscountHandler = applyDiscountHandler;
        _cancelHandler = cancelHandler;
        _createHandler = createHandler;
        _markAsFailedHandler = markAsFailedHandler;
        _markAsPaidHandler = markAsPaidHandler;
        _removeItemHandler = removeItemHandler;

        _getByIdHandler = getByIdHandler;
        _getByOrderNumberHandler = getByOrderNumberHandler;
        _getByUserIdHandler = getByUserIdHandler;
        _getByUserIdAndStatusHandler = getByUserIdAndStatusHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new CreateOrderCommand(
            userId,
            request.OrderNumber);

        var orderId = await _createHandler
            .Handle(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { orderId },
            new { orderId });
    }

    [HttpPost("{orderId:int}/items")]
    public async Task<IActionResult> AddItem(
        int orderId,
        [FromBody] AddItemToOrderRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new AddItemToOrderCommand(
            userId,
            orderId,
            request.CourseId);

        await _addItemHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete("{orderId:int}/items")]
    public async Task<IActionResult> RemoveItem(
        int orderId,
        [FromBody] RemoveItemFromOrderRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new RemoveItemFromOrderCommand(
            userId,
            orderId,
            request.CourseId);

        await _removeItemHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPost("{orderId:int}/discount")]
    public async Task<IActionResult> ApplyDiscount(
        int orderId,
        [FromBody] ApplyDiscountToOrderRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new ApplyDiscountToOrderCommand(
            userId,
            orderId,
            request.DiscountCode);

        await _applyDiscountHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{orderId:int}/cancel")]
    public async Task<IActionResult> Cancel(
        int orderId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new CancelOrderCommand(
            userId,
            orderId);

        await _cancelHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{orderId:int}/paid")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsPaid(
        int orderId,
        CancellationToken cancellationToken)
    {
        var command = new MarkOrderAsPaidCommand(orderId);

        await _markAsPaidHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{orderId:int}/failed")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsFailed(
        int orderId,
        CancellationToken cancellationToken)
    {
        var command = new MarkOrderAsFailedCommand(orderId);

        await _markAsFailedHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = GetUserId();

        var query = new GetOrdersByUserIdQuery(userId);

        var result = await _getByUserIdHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("my/status/{status}")]
    public async Task<IActionResult> GetMyOrdersByStatus(
        OrderStatus status)
    {
        var userId = GetUserId();

        var query = new GetOrdersByUserIdAndStatusQuery(
            userId,
            status);

        var result = await _getByUserIdAndStatusHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("{orderId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(
        int orderId)
    {
        var query = new GetOrderByIdQuery(orderId);

        var result = await _getByIdHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("number/{orderNumber}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByOrderNumber(
        string orderNumber)
    {
        var query = new GetOrderByOrderNumberQuery(
            orderNumber);

        var result = await _getByOrderNumberHandler
            .Handle(query);

        return Ok(result);
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(
            ClaimTypes.NameIdentifier);

        if (userIdClaim is null ||
            !int.TryParse(userIdClaim.Value, out var userId) ||
            userId <= 0)
        {
            throw new UnauthorizedException(
                "شناسه کاربر در توکن معتبر نیست");
        }

        return userId;
    }
}