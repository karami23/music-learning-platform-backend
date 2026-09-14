using MusicEducation.Domain.ValueObjects;

namespace MusicEducation.Application.Interfaces;

public interface ISmsService
{
    Task SendVerificationCodeAsync(
        PhoneNumber phoneNumber,
        string verificationCode);
}