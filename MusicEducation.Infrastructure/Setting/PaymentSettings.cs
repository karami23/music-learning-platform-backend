namespace MusicEducation.Infrastructure.Settings;

public class PaymentSettings
{
    public const string SectionName = "Payment";

    public string Provider { get; set; } = null!;

    public string MerchantId { get; set; } = null!;

    public string CallbackUrl { get; set; } = null!;
}