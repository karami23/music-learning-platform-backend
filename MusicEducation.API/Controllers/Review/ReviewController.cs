using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.API.DTOs.Reviews;
using MusicEducation.Application.Commands.Reviews.ApproveReview;
using MusicEducation.Application.Commands.Reviews.CreateReview;
using MusicEducation.Application.Commands.Reviews.RejectReview;
using MusicEducation.Application.Commands.Reviews.UpdateReview;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Queries.Reviews.GetApprovedReviewsByCourseId;
using MusicEducation.Application.Queries.Reviews.GetReviewById;
using MusicEducation.Application.Queries.Reviews.GetReviewByUserAndCourse;
using MusicEducation.Application.Queries.Reviews.GetReviewsByCourseId;
using MusicEducation.Application.Queries.Reviews.GetReviewsByUserId;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class ReviewController : ControllerBase
{
    private readonly ApproveReviewCommandHandler _approveHandler;
    private readonly CreateReviewCommandHandler _createHandler;
    private readonly RejectReviewCommandHandler _rejectHandler;
    private readonly UpdateReviewCommandHandler _updateHandler;

    private readonly GetApprovedReviewsByCourseIdQueryHandler _getApprovedByCourseHandler;
    private readonly GetReviewByIdQueryHandler _getByIdHandler;
    private readonly GetReviewByUserAndCourseQueryHandler _getByUserAndCourseHandler;
    private readonly GetReviewsByCourseIdQueryHandler _getByCourseHandler;
    private readonly GetReviewsByUserIdQueryHandler _getByUserHandler;

    public ReviewController(
        ApproveReviewCommandHandler approveHandler,
        CreateReviewCommandHandler createHandler,
        RejectReviewCommandHandler rejectHandler,
        UpdateReviewCommandHandler updateHandler,
        GetApprovedReviewsByCourseIdQueryHandler getApprovedByCourseHandler,
        GetReviewByIdQueryHandler getByIdHandler,
        GetReviewByUserAndCourseQueryHandler getByUserAndCourseHandler,
        GetReviewsByCourseIdQueryHandler getByCourseHandler,
        GetReviewsByUserIdQueryHandler getByUserHandler)
    {
        _approveHandler = approveHandler;
        _createHandler = createHandler;
        _rejectHandler = rejectHandler;
        _updateHandler = updateHandler;

        _getApprovedByCourseHandler = getApprovedByCourseHandler;
        _getByIdHandler = getByIdHandler;
        _getByUserAndCourseHandler = getByUserAndCourseHandler;
        _getByCourseHandler = getByCourseHandler;
        _getByUserHandler = getByUserHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new CreateReviewCommand(
            userId,
            request.CourseId,
            request.Rating,
            request.Comment);

        var result = await _createHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{reviewId:int}")]
    public async Task<IActionResult> Update(
        int reviewId,
        [FromBody] UpdateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();

        var command = new UpdateReviewCommand(
            userId,
            reviewId,
            request.Rating,
            request.Comment);

        var result = await _updateHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{reviewId:int}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Approve(
        int reviewId,
        CancellationToken cancellationToken)
    {
        var command = new ApproveReviewCommand(reviewId);

        var result = await _approveHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{reviewId:int}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Reject(
        int reviewId,
        CancellationToken cancellationToken)
    {
        var command = new RejectReviewCommand(reviewId);

        var result = await _rejectHandler.Handle(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMy()
    {
        var userId = GetUserId();

        var query = new GetReviewsByUserIdQuery(userId);

        var result = await _getByUserHandler.Handle(query);

        return Ok(result);
    }

    [HttpGet("my/course/{courseId:int}")]
    public async Task<IActionResult> GetMyByCourse(
        int courseId)
    {
        var userId = GetUserId();

        var query = new GetReviewByUserAndCourseQuery(
            userId,
            courseId);

        var result = await _getByUserAndCourseHandler.Handle(query);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("course/{courseId:int}/approved")]
    [AllowAnonymous]
    public async Task<IActionResult> GetApprovedByCourse(
        int courseId)
    {
        var query = new GetApprovedReviewsByCourseIdQuery(
            courseId);

        var result = await _getApprovedByCourseHandler.Handle(query);

        return Ok(result);
    }

    [HttpGet("course/{courseId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByCourse(
        int courseId)
    {
        var query = new GetReviewsByCourseIdQuery(
            courseId);

        var result = await _getByCourseHandler.Handle(query);

        return Ok(result);
    }

    [HttpGet("{reviewId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(
        int reviewId)
    {
        var query = new GetReviewByIdQuery(reviewId);

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