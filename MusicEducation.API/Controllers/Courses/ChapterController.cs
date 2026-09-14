using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MusicEducation.API.DTOs.Courses.Chapter;

using MusicEducation.Application.Commands.Courses.Chapters.ChangeChapterOrder;
using MusicEducation.Application.Commands.Courses.Chapters.CreateChapter;
using MusicEducation.Application.Commands.Courses.Chapters.UpdateChapter;

using MusicEducation.Application.Queries.Courses.Chapters;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ChapterController : ControllerBase
{
    private readonly ChangeChapterOrderHandler _changeChapterOrderHandler;
    private readonly CreateChapterHandler _createChapterHandler;
    private readonly UpdateChapterHandler _updateChapterHandler;

    private readonly GetAllChaptersQueryHandler _getAllChaptersHandler;
    private readonly GetChapterByIdQueryHandler _getChapterByIdHandler;
    private readonly GetChaptersByCourseIdQueryHandler _getChaptersByCourseIdHandler;

    public ChapterController(
        ChangeChapterOrderHandler changeChapterOrderHandler,
        CreateChapterHandler createChapterHandler,
        UpdateChapterHandler updateChapterHandler,
        GetAllChaptersQueryHandler getAllChaptersHandler,
        GetChapterByIdQueryHandler getChapterByIdHandler,
        GetChaptersByCourseIdQueryHandler getChaptersByCourseIdHandler)
    {
        _changeChapterOrderHandler = changeChapterOrderHandler;
        _createChapterHandler = createChapterHandler;
        _updateChapterHandler = updateChapterHandler;

        _getAllChaptersHandler = getAllChaptersHandler;
        _getChapterByIdHandler = getChapterByIdHandler;
        _getChaptersByCourseIdHandler = getChaptersByCourseIdHandler;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateChapterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateChapterCommand(
            request.CourseId,
            request.Title,
            request.Description,
            request.Order);

        var chapterId = await _createChapterHandler.HandleAsync(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { chapterId },
            new { chapterId });
    }

    [HttpPut("{chapterId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int chapterId,
        [FromBody] UpdateChapterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateChapterCommand(
            chapterId,
            request.Title,
            request.Description,
            request.Order);

        await _updateChapterHandler.HandleAsync(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{chapterId:int}/order")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeOrder(
        int chapterId,
        [FromBody] ChangeChapterOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangeChapterOrderCommand(
            chapterId,
            request.Order);

        await _changeChapterOrderHandler.HandleAsync(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var query = new GetAllChaptersQuery();

        var result = await _getAllChaptersHandler.Handle(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{chapterId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(
        int chapterId,
        CancellationToken cancellationToken)
    {
        var query = new GetChapterByIdQuery(chapterId);

        var result = await _getChapterByIdHandler.Handle(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("course/{courseId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCourseId(
        int courseId,
        CancellationToken cancellationToken)
    {
        var query = new GetChaptersByCourseIdQuery(courseId);

        var result = await _getChaptersByCourseIdHandler.Handle(
            query,
            cancellationToken);

        return Ok(result);
    }
}