using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.Application.Commands.Learning.CourseAccess.RestoreCourseAccess;
using MusicEducation.Application.Commands.Learning.CourseAccess.RevokeCourseAccess;
using MusicEducation.Application.Exceptions;
using MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessById;
using MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessByUserAndCourse;
using MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessesByCourseId;
using MusicEducation.Application.Queries.Learning.CourseAccess.GetCourseAccessesByUserId;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CourseAccessController : ControllerBase
{
    private readonly RestoreCourseAccessCommandHandler _restoreHandler;
    private readonly RevokeCourseAccessCommandHandler _revokeHandler;

    private readonly GetCourseAccessByIdQueryHandler _getByIdHandler;
    private readonly GetCourseAccessByUserAndCourseQueryHandler _getByUserAndCourseHandler;
    private readonly GetCourseAccessesByCourseIdQueryHandler _getByCourseIdHandler;
    private readonly GetCourseAccessesByUserIdQueryHandler _getByUserIdHandler;

    public CourseAccessController(
        RestoreCourseAccessCommandHandler restoreHandler,
        RevokeCourseAccessCommandHandler revokeHandler,
        GetCourseAccessByIdQueryHandler getByIdHandler,
        GetCourseAccessByUserAndCourseQueryHandler getByUserAndCourseHandler,
        GetCourseAccessesByCourseIdQueryHandler getByCourseIdHandler,
        GetCourseAccessesByUserIdQueryHandler getByUserIdHandler)
    {
        _restoreHandler = restoreHandler;
        _revokeHandler = revokeHandler;

        _getByIdHandler = getByIdHandler;
        _getByUserAndCourseHandler = getByUserAndCourseHandler;
        _getByCourseIdHandler = getByCourseIdHandler;
        _getByUserIdHandler = getByUserIdHandler;
    }

    [HttpPut("{courseAccessId:int}/restore")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Restore(
        int courseAccessId,
        CancellationToken cancellationToken)
    {
        var command = new RestoreCourseAccessCommand(
            courseAccessId);

        var result = await _restoreHandler
            .Handle(command, cancellationToken);

        return Ok(result);
    }

    [HttpPut("{courseAccessId:int}/revoke")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Revoke(
        int courseAccessId,
        CancellationToken cancellationToken)
    {
        var command = new RevokeCourseAccessCommand(
            courseAccessId);

        var result = await _revokeHandler
            .Handle(command, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{courseAccessId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(
        int courseAccessId)
    {
        var query = new GetCourseAccessByIdQuery(
            courseAccessId);

        var result = await _getByIdHandler
            .Handle(query);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("my/course/{courseId:int}")]
    public async Task<IActionResult> GetMyCourseAccess(
        int courseId)
    {
        var userId = GetUserId();

        var query = new GetCourseAccessByUserAndCourseQuery(
            userId,
            courseId);

        var result = await _getByUserAndCourseHandler
            .Handle(query);

        if (result is null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("course/{courseId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByCourseId(
        int courseId)
    {
        var query = new GetCourseAccessesByCourseIdQuery(
            courseId);

        var result = await _getByCourseIdHandler
            .Handle(query);

        return Ok(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyCourseAccesses()
    {
        var userId = GetUserId();

        var query = new GetCourseAccessesByUserIdQuery(
            userId);

        var result = await _getByUserIdHandler
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