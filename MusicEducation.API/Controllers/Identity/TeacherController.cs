using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MusicEducation.API.DTOs.Teachers;

using MusicEducation.Application.Commands.Identity.Teacher.ActivateTeacher;
using MusicEducation.Application.Commands.Identity.Teacher.DeactivateTeacher;
using MusicEducation.Application.Commands.Identity.Teacher.UpdateProfile;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class TeacherController : ControllerBase
{
    private readonly ActivateTeacherHandler _activateTeacherHandler;
    private readonly DeactivateTeacherHandler _deactivateTeacherHandler;
    private readonly UpdateTeacherProfileHandler _updateTeacherProfileHandler;

    public TeacherController(
        ActivateTeacherHandler activateTeacherHandler,
        DeactivateTeacherHandler deactivateTeacherHandler,
        UpdateTeacherProfileHandler updateTeacherProfileHandler)
    {
        _activateTeacherHandler = activateTeacherHandler;
        _deactivateTeacherHandler = deactivateTeacherHandler;
        _updateTeacherProfileHandler = updateTeacherProfileHandler;
    }

    [HttpPut("{teacherId:int}/activate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActivateTeacher(
        int teacherId,
        CancellationToken cancellationToken)
    {
        var command = new ActivateTeacherCommand(
            teacherId);

        await _activateTeacherHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{teacherId:int}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeactivateTeacher(
        int teacherId,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateTeacherCommand(
            teacherId);

        await _deactivateTeacherHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("profile")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateTeacherProfileRequest request,
        CancellationToken cancellationToken)
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var command = new UpdateTeacherProfileCommand(
            userId,
            request.Bio);

        await _updateTeacherProfileHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }
}