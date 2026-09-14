namespace MusicEducation.Infrastructure.Settings;

public class SmsSettings
{
    public const string SectionName = "Sms";

    public string Provider { get; set; } = null!;

    public string ApiKey { get; set; } = null!;

    public string Sender { get; set; } = null!;
}