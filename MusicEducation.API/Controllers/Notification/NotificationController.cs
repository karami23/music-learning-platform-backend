using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.API.DTOs.Notifications;
using MusicEducation.Application.Commands.Notifications.CreateNotification;
using MusicEducation.Application.Commands.Notifications.MarkNotificationAsRead;
using MusicEducation.Application.Commands.Notifications.MarkNotificationAsUnread;
using MusicEducation.Application.Commands.Notifications.UpdateNotification;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Queries.Notifications.GetNotificationById;
using MusicEducation.Application.Queries.Notifications.GetNotificationsByUserId;
using MusicEducation.Application.Queries.Notifications.GetUnreadNotificationCount;
using MusicEducation.Application.Queries.Notifications.GetUnreadNotificationsByUserId;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class NotificationController : ControllerBase
{
    private readonly CreateNotificationCommandHandler _createHandler;
    private readonly MarkNotificationAsReadCommandHandler _markAsReadHandler;
    private readonly MarkNotificationAsUnreadCommandHandler _markAsUnreadHandler;
    private readonly UpdateNotificationCommandHandler _updateHandler;

    private readonly GetNotificationByIdQueryHandler _getByIdHandler;
    private readonly GetNotificationsByUserIdQueryHandler _getByUserIdHandler;
    private readonly GetUnreadNotificationCountQueryHandler _getUnreadCountHandler;
    private readonly GetUnreadNotificationsByUserIdQueryHandler _getUnreadByUserIdHandler;

    public NotificationController(
        CreateNotificationCommandHandler createHandler,
        MarkNotificationAsReadCommandHandler markAsReadHandler,
        MarkNotificationAsUnreadCommandHandler markAsUnreadHandler,
        UpdateNotificationCommandHandler updateHandler,
        GetNotificationByIdQueryHandler getByIdHandler,
        GetNotificationsByUserIdQueryHandler getByUserIdHandler,
        GetUnreadNotificationCountQueryHandler getUnreadCountHandler,
        GetUnreadNotificationsByUserIdQueryHandler getUnreadByUserIdHandler)
    {
        _createHandler = createHandler;
        _markAsReadHandler = markAsReadHandler;
        _markAsUnreadHandler = markAsUnreadHandler;
        _updateHandler = updateHandler;

        _getByIdHandler = getByIdHandler;
        _getByUserIdHandler = getByUserIdHandler;
        _getUnreadCountHandler = getUnreadCountHandler;
        _getUnreadByUserIdHandler = getUnreadByUserIdHandler;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateNotificationCommand(
            request.UserId,
            request.Type,
            request.Title,
            request.Message,
            request.ActionUrl);

        var result = await _createHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{notificationId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int notificationId,
        [FromBody] UpdateNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateNotificationCommand(
            notificationId,
            request.Type,
            request.Title,
            request.Message,
            request.ActionUrl);

        var result = await _updateHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{notificationId:int}/read")]
    public async Task<IActionResult> MarkAsRead(
        int notificationId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new MarkNotificationAsReadCommand(
            userId,
            notificationId);

        var result = await _markAsReadHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{notificationId:int}/unread")]
    public async Task<IActionResult> MarkAsUnread(
        int notificationId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new MarkNotificationAsUnreadCommand(
            userId,
            notificationId);

        var result = await _markAsUnreadHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMy()
    {
        var userId = GetUserId();

        var query = new GetNotificationsByUserIdQuery(userId);

        var result = await _getByUserIdHandler.Handle(query);

        return Ok(result);
    }

    [HttpGet("my/unread")]
    public async Task<IActionResult> GetMyUnread()
    {
        var userId = GetUserId();

        var query = new GetUnreadNotificationsByUserIdQuery(userId);

        var result = await _getUnreadByUserIdHandler.Handle(query);

        return Ok(result);
    }

    [HttpGet("my/unread/count")]
    public async Task<IActionResult> GetMyUnreadCount()
    {
        var userId = GetUserId();

        var query = new GetUnreadNotificationCountQuery(userId);

        var result = await _getUnreadCountHandler.Handle(query);

        return Ok(result);
    }

    [HttpGet("{notificationId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(
        int notificationId)
    {
        var query = new GetNotificationByIdQuery(
            notificationId);

        var result = await _getByIdHandler.Handle(query);

        if (result is null)
            return NotFound();

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