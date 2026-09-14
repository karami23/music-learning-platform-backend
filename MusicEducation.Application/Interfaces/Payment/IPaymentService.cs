namespace MusicEducation.Application.Interfaces.Payment;

public interface IPaymentService
{
    Task<string> CreatePaymentRequestAsync(
        int orderId,
        decimal amount,
        string callbackUrl);

    Task<PaymentVerificationResult> VerifyPaymentAsync(
        string transactionId,
        decimal amount);
}