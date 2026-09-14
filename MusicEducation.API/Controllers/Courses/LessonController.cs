using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MusicEducation.API.DTOs.Courses.Lesson;

using MusicEducation.Application.Commands.Courses.Lessons.ChangeLessonOrder;
using MusicEducation.Application.Commands.Courses.Lessons.CreateLesson;
using MusicEducation.Application.Commands.Courses.Lessons.UpdateLesson;

using MusicEducation.Application.Queries.Courses.Lessons;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class LessonController : ControllerBase
{
    private readonly ChangeLessonOrderCommandHandler _changeLessonOrderHandler;
    private readonly CreateLessonCommandHandler _createLessonHandler;
    private readonly UpdateLessonCommandHandler _updateLessonHandler;

    private readonly GetAllLessonsQueryHandler _getAllLessonsHandler;
    private readonly GetLessonByIdQueryHandler _getLessonByIdHandler;
    private readonly GetLessonsByChapterIdQueryHandler _getLessonsByChapterIdHandler;

    public LessonController(
        ChangeLessonOrderCommandHandler changeLessonOrderHandler,
        CreateLessonCommandHandler createLessonHandler,
        UpdateLessonCommandHandler updateLessonHandler,
        GetAllLessonsQueryHandler getAllLessonsHandler,
        GetLessonByIdQueryHandler getLessonByIdHandler,
        GetLessonsByChapterIdQueryHandler getLessonsByChapterIdHandler)
    {
        _changeLessonOrderHandler = changeLessonOrderHandler;
        _createLessonHandler = createLessonHandler;
        _updateLessonHandler = updateLessonHandler;

        _getAllLessonsHandler = getAllLessonsHandler;
        _getLessonByIdHandler = getLessonByIdHandler;
        _getLessonsByChapterIdHandler = getLessonsByChapterIdHandler;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] CreateLessonRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateLessonCommand(
            request.ChapterId,
            request.Title,
            request.Description,
            request.Order);

        var lessonId = await _createLessonHandler.Handle(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { lessonId },
            new { lessonId });
    }

    [HttpPut("{lessonId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int lessonId,
        [FromBody] UpdateLessonRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateLessonCommand(
            lessonId,
            request.Title,
            request.Description,
            request.Order);

        await _updateLessonHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{lessonId:int}/order")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeOrder(
        int lessonId,
        [FromBody] ChangeLessonOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangeLessonOrderCommand(
            lessonId,
            request.Order);

        await _changeLessonOrderHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var query = new GetAllLessonsQuery();

        var result = await _getAllLessonsHandler.Handle(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{lessonId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(
        int lessonId,
        CancellationToken cancellationToken)
    {
        var query = new GetLessonByIdQuery(lessonId);

        var result = await _getLessonByIdHandler.Handle(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("chapter/{chapterId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByChapterId(
        int chapterId,
        CancellationToken cancellationToken)
    {
        var query = new GetLessonsByChapterIdQuery(chapterId);

        var result = await _getLessonsByChapterIdHandler.Handle(
            query,
            cancellationToken);

        return Ok(result);
    }
}