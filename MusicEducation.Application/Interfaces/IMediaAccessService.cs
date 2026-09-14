namespace MusicEducation.Application.Interfaces;

public interface IMediaAccessService
{
    Task<string> GenerateAccessUrlAsync(
        string storageKey,
        TimeSpan expiration);
}