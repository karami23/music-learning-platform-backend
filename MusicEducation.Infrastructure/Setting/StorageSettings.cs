namespace MusicEducation.Infrastructure.Settings;

public class StorageSettings
{
    public const string SectionName = "Storage";

    public string RootPath { get; set; } = null!;
}