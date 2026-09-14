using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.API.DTOs.Commerce.Payment;
using MusicEducation.Application.Commands.Commerce.Payment;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Queries.Commerce.Payment;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PaymentController : ControllerBase
{
    private readonly CancelPaymentCommandHandler _cancelHandler;
    private readonly CreatePaymentCommandHandler _createHandler;
    private readonly MarkPaymentAsFailedCommandHandler _markAsFailedHandler;
    private readonly MarkPaymentAsPaidCommandHandler _markAsPaidHandler;

    private readonly GetPaymentByIdQueryHandler _getByIdHandler;
    private readonly GetPaymentByOrderIdQueryHandler _getByOrderIdHandler;
    private readonly GetPaymentByTransactionIdQueryHandler _getByTransactionIdHandler;
    private readonly GetPaymentsByUserIdQueryHandler _getByUserIdHandler;

    public PaymentController(
        CancelPaymentCommandHandler cancelHandler,
        CreatePaymentCommandHandler createHandler,
        MarkPaymentAsFailedCommandHandler markAsFailedHandler,
        MarkPaymentAsPaidCommandHandler markAsPaidHandler,
        GetPaymentByIdQueryHandler getByIdHandler,
        GetPaymentByOrderIdQueryHandler getByOrderIdHandler,
        GetPaymentByTransactionIdQueryHandler getByTransactionIdHandler,
        GetPaymentsByUserIdQueryHandler getByUserIdHandler)
    {
        _cancelHandler = cancelHandler;
        _createHandler = createHandler;
        _markAsFailedHandler = markAsFailedHandler;
        _markAsPaidHandler = markAsPaidHandler;

        _getByIdHandler = getByIdHandler;
        _getByOrderIdHandler = getByOrderIdHandler;
        _getByTransactionIdHandler = getByTransactionIdHandler;
        _getByUserIdHandler = getByUserIdHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreatePaymentRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new CreatePaymentCommand(
            userId,
            request.OrderId);

        var paymentId = await _createHandler
            .Handle(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { paymentId },
            new { paymentId });
    }

    [HttpPut("{paymentId:int}/cancel")]
    public async Task<IActionResult> Cancel(
        int paymentId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new CancelPaymentCommand(
            userId,
            paymentId);

        await _cancelHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{paymentId:int}/paid")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsPaid(
        int paymentId,
        [FromBody] MarkPaymentAsPaidRequest request,
        CancellationToken cancellationToken)
    {
        var command = new MarkPaymentAsPaidCommand(
            paymentId,
            request.TransactionId,
            request.ReferenceNumber);

        await _markAsPaidHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpPut("{paymentId:int}/failed")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsFailed(
        int paymentId,
        CancellationToken cancellationToken)
    {
        var command = new MarkPaymentAsFailedCommand(
            paymentId);

        await _markAsFailedHandler
            .Handle(command, cancellationToken);

        return NoContent();
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyPayments()
    {
        var userId = GetUserId();

        var query = new GetPaymentsByUserIdQuery(userId);

        var result = await _getByUserIdHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("{paymentId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(
        int paymentId)
    {
        var query = new GetPaymentByIdQuery(
            paymentId);

        var result = await _getByIdHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("order/{orderId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByOrderId(
        int orderId)
    {
        var query = new GetPaymentByOrderIdQuery(
            orderId);

        var result = await _getByOrderIdHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("transaction/{transactionId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByTransactionId(
        string transactionId)
    {
        var query = new GetPaymentByTransactionIdQuery(
            transactionId);

        var result = await _getByTransactionIdHandler
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