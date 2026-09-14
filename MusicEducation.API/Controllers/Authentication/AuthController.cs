using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicEducation.API.DTOs.Authentication;
using MusicEducation.Application.Commands.Authentication.ForgotPassword;
using MusicEducation.Application.Commands.Authentication.Login;
using MusicEducation.Application.Commands.Authentication.LoginType;
using MusicEducation.Application.Commands.Authentication.Logout;
using MusicEducation.Application.Commands.Authentication.RefreshToken;
using MusicEducation.Application.Commands.Authentication.Register;
using MusicEducation.Application.Commands.Authentication.ResetPassword;
using MusicEducation.Application.Commands.Authentication.SendVerificationCode;
using MusicEducation.Application.Commands.Authentication.VerifyPhone;

namespace MusicEducation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly RegisterUserHandler _registerUserHandler;
    private readonly LoginHandler _loginHandler;
    private readonly LogoutHandler _logoutHandler;
    private readonly RefreshTokenHandler _refreshTokenHandler;
    private readonly SendVerificationCodeHandler _sendVerificationCodeHandler;
    private readonly VerifyPhoneHandler _verifyPhoneHandler;
    private readonly ForgotPasswordHandler _forgotPasswordHandler;
    private readonly ResetPasswordHandler _resetPasswordHandler;

    public AuthenticationController(
        RegisterUserHandler registerUserHandler,
        LoginHandler loginHandler,
        LogoutHandler logoutHandler,
        RefreshTokenHandler refreshTokenHandler,
        SendVerificationCodeHandler sendVerificationCodeHandler,
        VerifyPhoneHandler verifyPhoneHandler,
        ForgotPasswordHandler forgotPasswordHandler,
        ResetPasswordHandler resetPasswordHandler)
    {
        _registerUserHandler = registerUserHandler;
        _loginHandler = loginHandler;
        _logoutHandler = logoutHandler;
        _refreshTokenHandler = refreshTokenHandler;
        _sendVerificationCodeHandler = sendVerificationCodeHandler;
        _verifyPhoneHandler = verifyPhoneHandler;
        _forgotPasswordHandler = forgotPasswordHandler;
        _resetPasswordHandler = resetPasswordHandler;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.CountryCode,
            request.NationalNumber);

        await _registerUserHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(
            request.LoginType,
            request.Email,
            request.CountryCode,
            request.NationalNumber,
            request.Password);

        var response = await _loginHandler.Handle(
            command,
            cancellationToken);

        return Ok(response);
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LogoutCommand(
            request.RefreshToken);

        await _logoutHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(
            request.RefreshToken);

        var response = await _refreshTokenHandler.Handle(
            command,
            cancellationToken);

        return Ok(response);
    }

    [HttpPost("send-verification-code")]
    [AllowAnonymous]
    public async Task<IActionResult> SendVerificationCode(
        [FromBody] PhoneRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SendVerificationCodeCommand(
            request.CountryCode,
            request.NationalNumber);

        await _sendVerificationCodeHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("verify-phone")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyPhone(
        [FromBody] VerifyPhoneRequest request,
        CancellationToken cancellationToken)
    {
        var command = new VerifyPhoneCommand(
            request.CountryCode,
            request.NationalNumber,
            request.VerificationCode);

        await _verifyPhoneHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(
        [FromBody] PhoneRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ForgotPasswordCommand(
            request.CountryCode,
            request.NationalNumber);

        await _forgotPasswordHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ResetPasswordCommand(
            request.CountryCode,
            request.NationalNumber,
            request.VerificationCode,
            request.NewPassword);

        await _resetPasswordHandler.Handle(
            command,
            cancellationToken);

        return NoContent();
    }
}