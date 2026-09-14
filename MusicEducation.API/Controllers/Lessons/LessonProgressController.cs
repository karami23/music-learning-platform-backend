using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.API.DTOs.Learning.LessonProgress;
using MusicEducation.Application.Commands.Learning.LessonProgress.CreateLessonProgress;
using MusicEducation.Application.Commands.Learning.LessonProgress.MarkLessonAsCompleted;
using MusicEducation.Application.Commands.Learning.LessonProgress.ResetLessonProgress;
using MusicEducation.Application.Commands.Learning.LessonProgress.UpdateLessonProgress;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Queries.Learning.LessonProgress.GetLastLessonProgress;
using MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressById;
using MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressByUserAndLesson;
using MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressesByLessonId;
using MusicEducation.Application.Queries.Learning.LessonProgress.GetLessonProgressesByUserId;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class LessonProgressController : ControllerBase
{
    private readonly CreateLessonProgressCommandHandler _createHandler;
    private readonly MarkLessonAsCompletedCommandHandler _completeHandler;
    private readonly ResetLessonProgressCommandHandler _resetHandler;
    private readonly UpdateLessonProgressCommandHandler _updateHandler;

    private readonly GetLastLessonProgressQueryHandler _getLastHandler;
    private readonly GetLessonProgressByIdQueryHandler _getByIdHandler;
    private readonly GetLessonProgressByUserAndLessonQueryHandler _getByUserAndLessonHandler;
    private readonly GetLessonProgressesByLessonIdQueryHandler _getByLessonIdHandler;
    private readonly GetLessonProgressesByUserIdQueryHandler _getByUserIdHandler;

    public LessonProgressController(
        CreateLessonProgressCommandHandler createHandler,
        MarkLessonAsCompletedCommandHandler completeHandler,
        ResetLessonProgressCommandHandler resetHandler,
        UpdateLessonProgressCommandHandler updateHandler,
        GetLastLessonProgressQueryHandler getLastHandler,
        GetLessonProgressByIdQueryHandler getByIdHandler,
        GetLessonProgressByUserAndLessonQueryHandler getByUserAndLessonHandler,
        GetLessonProgressesByLessonIdQueryHandler getByLessonIdHandler,
        GetLessonProgressesByUserIdQueryHandler getByUserIdHandler)
    {
        _createHandler = createHandler;
        _completeHandler = completeHandler;
        _resetHandler = resetHandler;
        _updateHandler = updateHandler;

        _getLastHandler = getLastHandler;
        _getByIdHandler = getByIdHandler;
        _getByUserAndLessonHandler = getByUserAndLessonHandler;
        _getByLessonIdHandler = getByLessonIdHandler;
        _getByUserIdHandler = getByUserIdHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateLessonProgressRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new CreateLessonProgressCommand(
            userId,
            request.LessonId);

        var result = await _createHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{lessonProgressId:int}")]
    public async Task<IActionResult> Update(
        int lessonProgressId,
        [FromBody] UpdateLessonProgressRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new UpdateLessonProgressCommand(
            userId,
            lessonProgressId,
            request.MediaId,
            request.Position,
            request.ProgressPercentage);

        var result = await _updateHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{lessonProgressId:int}/complete")]
    public async Task<IActionResult> Complete(
        int lessonProgressId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new MarkLessonAsCompletedCommand(
            userId,
            lessonProgressId);

        var result = await _completeHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{lessonProgressId:int}/reset")]
    public async Task<IActionResult> Reset(
        int lessonProgressId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new ResetLessonProgressCommand(
            userId,
            lessonProgressId);

        var result = await _resetHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMy()
    {
        var userId = GetUserId();

        var query = new GetLessonProgressesByUserIdQuery(userId);

        var result = await _getByUserIdHandler.Handle(query);

        return Ok(result);
    }

    [HttpGet("my/lesson/{lessonId:int}")]
    public async Task<IActionResult> GetMyByLesson(
        int lessonId)
    {
        var userId = GetUserId();

        var query = new GetLessonProgressByUserAndLessonQuery(
            userId,
            lessonId);

        var result = await _getByUserAndLessonHandler.Handle(query);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("my/course/{courseId:int}/last")]
    public async Task<IActionResult> GetMyLast(
        int courseId)
    {
        var userId = GetUserId();

        var query = new GetLastLessonProgressQuery(
            userId,
            courseId);

        var result = await _getLastHandler.Handle(query);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("{lessonProgressId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(
        int lessonProgressId)
    {
        var query = new GetLessonProgressByIdQuery(
            lessonProgressId);

        var result = await _getByIdHandler.Handle(query);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("lesson/{lessonId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByLessonId(
        int lessonId)
    {
        var query = new GetLessonProgressesByLessonIdQuery(
            lessonId);

        var result = await _getByLessonIdHandler.Handle(query);

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