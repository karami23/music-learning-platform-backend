namespace MusicEducation.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        string folder);

    Task DeleteAsync(string storageKey);

    Task<bool> ExistsAsync(string storageKey);
}
