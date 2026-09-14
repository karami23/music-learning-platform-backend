using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MusicEducation.API.DTOs.Users;

using MusicEducation.Application.Commands.Identity.User.ActivateUser;
using MusicEducation.Application.Commands.Identity.User.ChangePassword;
using MusicEducation.Application.Commands.Identity.User.DeactivateUser;
using MusicEducation.Application.Commands.Identity.User.PromoteUserToTeacher;
using MusicEducation.Application.Commands.Identity.User.UpdateProfile;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class UserController : ControllerBase
{
    private readonly UpdateProfileHandler _updateProfileHandler;
    private readonly ChangePasswordHandler _changePasswordHandler;
    private readonly ActivateUserHandler _activateUserHandler;
    private readonly DeactivateUserHandler _deactivateUserHandler;
    private readonly PromoteUserToTeacherHandler _promoteUserToTeacherHandler;

    public UserController(
        UpdateProfileHandler updateProfileHandler,
        ChangePasswordHandler changePasswordHandler,
        ActivateUserHandler activateUserHandler,
        DeactivateUserHandler deactivateUserHandler,
        PromoteUserToTeacherHandler promoteUserToTeacherHandler)
    {
        _updateProfileHandler = updateProfileHandler;
        _changePasswordHandler = changePasswordHandler;
        _activateUserHandler = activateUserHandler;
        _deactivateUserHandler = deactivateUserHandler;
        _promoteUserToTeacherHandler = promoteUserToTeacherHandler;
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateProfileRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var command = new UpdateProfileCommand(
            userId,
            request.FirstName,
            request.LastName,
            request.CountryCode,
            request.NationalNumber,
            request.ProfileImageUrl);

        await _updateProfileHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var command = new ChangePasswordCommand(
            userId,
            request.CurrentPassword,
            request.NewPassword);

        await _changePasswordHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{userId:int}/activate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActivateUser(
        int userId,
        CancellationToken cancellationToken)
    {
        var command = new ActivateUserCommand(userId);

        await _activateUserHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{userId:int}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeactivateUser(
        int userId,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateUserCommand(userId);

        await _deactivateUserHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPut("{userId:int}/promote-to-teacher")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PromoteUserToTeacher(
        int userId,
        [FromBody] PromoteUserToTeacherRequest request,
        CancellationToken cancellationToken)
    {
        var command = new PromoteUserToTeacherCommand(
            userId,
            request.Bio);

        await _promoteUserToTeacherHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    private int GetCurrentUserId()
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (!int.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("شناسه کاربر معتبر نیست");
        }

        return userId;
    }
}