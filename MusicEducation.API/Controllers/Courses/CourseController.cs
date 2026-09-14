using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.API.DTOs.Courses;
using MusicEducation.API.DTOs.Courses.Course;
using MusicEducation.Application.Commands.Courses.Course.ArchiveCourse;
using MusicEducation.Application.Commands.Courses.Course.ChangeCoursePrice;
using MusicEducation.Application.Commands.Courses.Course.ChangeCoursePricingType;
using MusicEducation.Application.Commands.Courses.Course.CreateCourse;
using MusicEducation.Application.Commands.Courses.Course.CreateMyCourse;
using MusicEducation.Application.Commands.Courses.Course.PublishCourse;
using MusicEducation.Application.Commands.Courses.Course.ReturnCourseToDraft;
using MusicEducation.Application.Commands.Courses.Course.UpdateCourse;
using MusicEducation.Application.Commands.Courses.Course.UpdateMyCourse;
using MusicEducation.Application.Commands.Courses.RejectCourse;
using MusicEducation.Application.Commands.Courses.SubmitCourseForReview;
using MusicEducation.Application.Queries.Courses;
using System.Security.Claims;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CourseController : ControllerBase
{
    private readonly ArchiveCourseHandler _archiveCourseHandler;
    private readonly ChangeCoursePriceHandler _changeCoursePriceHandler;
    private readonly ChangeCoursePricingTypeHandler _changeCoursePricingTypeHandler;

    private readonly CreateCourseHandler _createCourseHandler;
    private readonly CreateMyCourseHandler _createMyCourseHandler;

    private readonly PublishCourseHandler _publishCourseHandler;
    private readonly ReturnCourseToDraftHandler _returnCourseToDraftHandler;

    private readonly UpdateCourseHandler _updateCourseHandler;
    private readonly UpdateMyCourseHandler _updateMyCourseHandler;

    private readonly RejectCourseCommandHandler _rejectCourseHandler;
    private readonly SubmitCourseForReviewCommandHandler _submitCourseForReviewHandler;

    private readonly GetCourseByIdHandler _getCourseByIdHandler;
    private readonly GetCourseBySlugHandler _getCourseBySlugHandler;
    private readonly GetCoursesHandler _getCoursesHandler;

    public CourseController(
        ArchiveCourseHandler archiveCourseHandler,
        ChangeCoursePriceHandler changeCoursePriceHandler,
        ChangeCoursePricingTypeHandler changeCoursePricingTypeHandler,
        CreateCourseHandler createCourseHandler,
        CreateMyCourseHandler createMyCourseHandler,
        PublishCourseHandler publishCourseHandler,
        ReturnCourseToDraftHandler returnCourseToDraftHandler,
        UpdateCourseHandler updateCourseHandler,
        UpdateMyCourseHandler updateMyCourseHandler,
        RejectCourseCommandHandler rejectCourseHandler,
        SubmitCourseForReviewCommandHandler submitCourseForReviewHandler,
        GetCourseByIdHandler getCourseByIdHandler,
        GetCourseBySlugHandler getCourseBySlugHandler,
        GetCoursesHandler getCoursesHandler)
    {
        _archiveCourseHandler = archiveCourseHandler;
        _changeCoursePriceHandler = changeCoursePriceHandler;
        _changeCoursePricingTypeHandler = changeCoursePricingTypeHandler;

        _createCourseHandler = createCourseHandler;
        _createMyCourseHandler = createMyCourseHandler;

        _publishCourseHandler = publishCourseHandler;
        _returnCourseToDraftHandler = returnCourseToDraftHandler;

        _updateCourseHandler = updateCourseHandler;
        _updateMyCourseHandler = updateMyCourseHandler;

        _rejectCourseHandler = rejectCourseHandler;
        _submitCourseForReviewHandler = submitCourseForReviewHandler;

        _getCourseByIdHandler = getCourseByIdHandler;
        _getCourseBySlugHandler = getCourseBySlugHandler;
        _getCoursesHandler = getCoursesHandler;
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> CreateMyCourse(
        [FromBody] CreateCourseRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var command = new CreateMyCourseCommand(
            userId,
            request.CourseCategoryId,
            request.Title,
            request.Slug,
            request.Description,
            request.PricingType,
            request.PriceAmount,
            request.CoverImageUrl);

        var courseId = await _createMyCourseHandler.HandleAsync(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { courseId },
            new { courseId });
    }

    [HttpPut("{courseId:int}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> UpdateMyCourse(
        int courseId,
        [FromBody] UpdateCourseRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var command = new UpdateMyCourseCommand(
            userId,
            courseId,
            request.CourseCategoryId,
            request.Title,
            request.Slug,
            request.Description,
            request.PricingType,
            request.PriceAmount,
            request.CoverImageUrl);

        await _updateMyCourseHandler.HandleAsync(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{courseId:int}/submit")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> SubmitForReview(
        int courseId)
    {
        var userId = GetCurrentUserId();

        var command = new SubmitCourseForReviewCommand(
            userId,
            courseId);

        await _submitCourseForReviewHandler.HandleAsync(
            command);

        return NoContent();
    }

    [HttpPost("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(
        [FromBody] AdminCreateCourseRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCourseCommand(
            request.TeacherId,
            request.CourseCategoryId,
            request.Title,
            request.Slug,
            request.Description,
            request.PricingType,
            request.PriceAmount,
            request.CoverImageUrl);

        var courseId = await _createCourseHandler.HandleAsync(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { courseId },
            new { courseId });
    }

    [HttpPut("admin/{courseId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int courseId,
        [FromBody] AdminUpdateCourseRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCourseCommand(
            courseId,
            request.TeacherId,
            request.CourseCategoryId,
            request.Title,
            request.Slug,
            request.Description,
            request.PricingType,
            request.PriceAmount,
            request.CoverImageUrl);

        await _updateCourseHandler.HandleAsync(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{courseId:int}/price")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangePrice(
        int courseId,
        [FromBody] ChangeCoursePriceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangeCoursePriceCommand(
            courseId,
            request.PriceAmount);

        await _changeCoursePriceHandler.HandleAsync(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{courseId:int}/pricing-type")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangePricingType(
        int courseId,
        [FromBody] ChangeCoursePricingTypeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangeCoursePricingTypeCommand(
            courseId,
            request.PricingType,
            request.PriceAmount);

        await _changeCoursePricingTypeHandler.HandleAsync(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{courseId:int}/publish")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Publish(
        int courseId,
        CancellationToken cancellationToken)
    {
        var command = new PublishCourseCommand(courseId);

        await _publishCourseHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{courseId:int}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Reject(
        int courseId)
    {
        var command = new RejectCourseCommand(courseId);

        await _rejectCourseHandler.HandleAsync(
            command);

        return NoContent();
    }

    [HttpPut("{courseId:int}/archive")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Archive(
        int courseId,
        CancellationToken cancellationToken)
    {
        var command = new ArchiveCourseCommand(courseId);

        await _archiveCourseHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{courseId:int}/return-to-draft")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ReturnToDraft(
        int courseId,
        CancellationToken cancellationToken)
    {
        var command = new ReturnCourseToDraftCommand(courseId);

        await _returnCourseToDraftHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCourses(
        [FromQuery] string? search,
        [FromQuery] int? teacherId,
        [FromQuery] int? courseCategoryId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetCoursesQuery(
            search,
            teacherId,
            courseCategoryId,
            page,
            pageSize);

        var result = await _getCoursesHandler.HandleAsync(query);

        return Ok(result);
    }

    [HttpGet("{courseId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(
        int courseId)
    {
        var query = new GetCourseByIdQuery(courseId);

        var result = await _getCourseByIdHandler.HandleAsync(query);

        return Ok(result);
    }

    [HttpGet("slug/{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySlug(
        string slug)
    {
        var query = new GetCourseBySlugQuery(slug);

        var result = await _getCourseBySlugHandler.HandleAsync(query);

        return Ok(result);
    }
    private int GetCurrentUserId()
    {
        var userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var userId))
            throw new UnauthorizedAccessException(
                "شناسه کاربر معتبر نیست");

        return userId;
    }
}