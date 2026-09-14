namespace MusicEducation.Application.DTOs.Authentication;

public record AuthenticationResponse(
    string AccessToken,
    string RefreshToken
);