using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.Domain.Enums;
using MusicEducation.API.DTOs.Courses.LessonMedia;

using MusicEducation.Application.Commands.Courses.LessonMedia.ChangeLessonMediaOrder;
using MusicEducation.Application.Commands.Courses.LessonMedia.CreateLessonMedia;
using MusicEducation.Application.Commands.Courses.LessonMedia.UpdateLessonMedia;

using MusicEducation.Application.Queries.Courses.Lessons.Media;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class LessonMediaController : ControllerBase
{
    private readonly ChangeLessonMediaOrderCommandHandler
        _changeLessonMediaOrderHandler;

    private readonly CreateLessonMediaCommandHandler
        _createLessonMediaHandler;

    private readonly UpdateLessonMediaCommandHandler
        _updateLessonMediaHandler;

    private readonly GetAllLessonMediaQueryHandler
        _getAllLessonMediaHandler;

    private readonly GetFirstLessonMediaQueryHandler
        _getFirstLessonMediaHandler;

    private readonly GetLessonMediaByIdQueryHandler
        _getLessonMediaByIdHandler;

    private readonly GetLessonMediaByLessonIdQueryHandler
        _getLessonMediaByLessonIdHandler;

    private readonly GetLessonMediaByLessonIdAndTypeQueryHandler
        _getLessonMediaByLessonIdAndTypeHandler;

    private readonly GetNextLessonMediaQueryHandler
        _getNextLessonMediaHandler;

    public LessonMediaController(
        ChangeLessonMediaOrderCommandHandler changeLessonMediaOrderHandler,
        CreateLessonMediaCommandHandler createLessonMediaHandler,
        UpdateLessonMediaCommandHandler updateLessonMediaHandler,
        GetAllLessonMediaQueryHandler getAllLessonMediaHandler,
        GetFirstLessonMediaQueryHandler getFirstLessonMediaHandler,
        GetLessonMediaByIdQueryHandler getLessonMediaByIdHandler,
        GetLessonMediaByLessonIdQueryHandler getLessonMediaByLessonIdHandler,
        GetLessonMediaByLessonIdAndTypeQueryHandler getLessonMediaByLessonIdAndTypeHandler,
        GetNextLessonMediaQueryHandler getNextLessonMediaHandler)
    {
        _changeLessonMediaOrderHandler =
            changeLessonMediaOrderHandler;

        _createLessonMediaHandler =
            createLessonMediaHandler;

        _updateLessonMediaHandler =
            updateLessonMediaHandler;

        _getAllLessonMediaHandler =
            getAllLessonMediaHandler;

        _getFirstLessonMediaHandler =
            getFirstLessonMediaHandler;

        _getLessonMediaByIdHandler =
            getLessonMediaByIdHandler;

        _getLessonMediaByLessonIdHandler =
            getLessonMediaByLessonIdHandler;

        _getLessonMediaByLessonIdAndTypeHandler =
            getLessonMediaByLessonIdAndTypeHandler;

        _getNextLessonMediaHandler =
            getNextLessonMediaHandler;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateLessonMediaRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateLessonMediaCommand(
            request.LessonId,
            request.MediaType,
            request.Title,
            request.FileName,
            request.StorageKey,
            request.Order,
            request.Duration.HasValue
                ? TimeSpan.FromSeconds(request.Duration.Value)
                : null);
        var mediaId = await _createLessonMediaHandler.Handle(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { mediaId },
            new { mediaId });
    }

    [HttpPut("{mediaId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int mediaId,
        [FromBody] UpdateLessonMediaRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLessonMediaCommand(
            mediaId,
            request.Title,
            request.FileName,
            request.StorageKey,
            request.Order,
            request.Duration.HasValue
                ? TimeSpan.FromSeconds(request.Duration.Value)
                : null);

        await _updateLessonMediaHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{mediaId:int}/order")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeOrder(
        int mediaId,
        [FromBody] ChangeLessonMediaOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangeLessonMediaOrderCommand(
            mediaId,
            request.Order);

        await _changeLessonMediaOrderHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var query = new GetAllLessonMediaQuery();

        var result = await _getAllLessonMediaHandler
            .HandleAsync(query);

        return Ok(result);
    }

    [HttpGet("{mediaId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(
        int mediaId)
    {
        var query = new GetLessonMediaByIdQuery(mediaId);

        var result = await _getLessonMediaByIdHandler
            .HandleAsync(query);

        return Ok(result);
    }

    [HttpGet("lesson/{lessonId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByLessonId(
        int lessonId)
    {
        var query = new GetLessonMediaByLessonIdQuery(lessonId);

        var result = await _getLessonMediaByLessonIdHandler
            .HandleAsync(query);

        return Ok(result);
    }

    [HttpGet("lesson/{lessonId:int}/type/{mediaType}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByLessonIdAndType(
    int lessonId,
    LessonMediaType mediaType)
    {
        var query = new GetLessonMediaByLessonIdAndTypeQuery(
            lessonId,
            mediaType);

        var result = await _getLessonMediaByLessonIdAndTypeHandler
            .HandleAsync(query);

        return Ok(result);
    }

    [HttpGet("lesson/{lessonId:int}/first")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFirst(
        int lessonId)
    {
        var query = new GetFirstLessonMediaQuery(lessonId);

        var result = await _getFirstLessonMediaHandler
            .HandleAsync(query);

        return Ok(result);
    }

    [HttpGet("{mediaId:int}/next")]
    [AllowAnonymous]
    public async Task<IActionResult> GetNext(
        int mediaId)
    {
        var query = new GetNextLessonMediaQuery(mediaId);

        var result = await _getNextLessonMediaHandler
            .HandleAsync(query);

        return Ok(result);
    }
}