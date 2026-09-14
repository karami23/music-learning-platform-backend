using Microsoft.Extensions.Options;
using MusicEducation.Application.Interfaces;
using MusicEducation.Domain.ValueObjects;
using MusicEducation.Infrastructure.Settings;

namespace MusicEducation.Infrastructure.Services.Sms;

public class SmsService : ISmsService
{
    private readonly SmsSettings _smsSettings;

    public SmsService(IOptions<SmsSettings> options)
    {
        _smsSettings = options.Value;
    }

    public Task SendVerificationCodeAsync(
        PhoneNumber phoneNumber,
        string verificationCode)
    {
        ArgumentNullException.ThrowIfNull(phoneNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(verificationCode);

        if (string.IsNullOrWhiteSpace(_smsSettings.Provider) ||
            _smsSettings.Provider == "TODO")
        {
            throw new InvalidOperationException("SMS Provider هنوز برای پروژه تنظیم نشده است.");
        }

        throw new NotImplementedException(
            "پیاده‌سازی ارسال SMS باید بر اساس Provider انتخاب‌شده انجام شود.");
    }
}