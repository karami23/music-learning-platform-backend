using Microsoft.Extensions.Options;
using MusicEducation.Application.Interfaces;
using MusicEducation.Application.Interfaces.Payment;
using MusicEducation.Infrastructure.Settings;

namespace MusicEducation.Infrastructure.Services.Payments;

public class PaymentService : IPaymentService
{
    private readonly PaymentSettings _paymentSettings;

    public PaymentService(IOptions<PaymentSettings> options)
    {
        _paymentSettings = options.Value;
    }
    public Task<string> CreatePaymentRequestAsync(
        int orderId,
        decimal amount,
        string callbackUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(callbackUrl);

        if (string.IsNullOrWhiteSpace(_paymentSettings.Provider) ||
            _paymentSettings.Provider == "TODO")
        {
            throw new InvalidOperationException(
                "Payment Provider هنوز برای پروژه تنظیم نشده است.");
        }

        throw new NotImplementedException(
            "پیاده‌سازی Payment باید بر اساس Provider انتخاب‌شده انجام شود.");
    }

    public Task<PaymentVerificationResult> VerifyPaymentAsync(
        string transactionId,
        decimal amount)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(transactionId);

        if (string.IsNullOrWhiteSpace(_paymentSettings.Provider) ||
            _paymentSettings.Provider == "TODO")
        {
            throw new InvalidOperationException(
                "Payment Provider هنوز برای پروژه تنظیم نشده است.");
        }

        throw new NotImplementedException(
            "پیاده‌سازی Verify Payment باید بر اساس Provider انتخاب‌شده انجام شود.");
    }
}