using Microsoft.Extensions.Options;
using MusicEducation.Application.Interfaces;
using MusicEducation.Infrastructure.Settings;

namespace MusicEducation.Infrastructure.Services.Storage;

public class FileStorageService : IFileStorageService
{
    private readonly StorageSettings _storageSettings;

    public FileStorageService(IOptions<StorageSettings> options)
    {
        _storageSettings = options.Value;
    }
    public async Task<string> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        string folder)
    {
        ArgumentNullException.ThrowIfNull(fileStream);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        ArgumentException.ThrowIfNullOrWhiteSpace(folder);

        if (!fileStream.CanRead)
        {
            throw new InvalidOperationException(
                "فایل قابل خواندن نیست.");
        }

        if (string.IsNullOrWhiteSpace(_storageSettings.RootPath))
        {
            throw new InvalidOperationException(
                "مسیر Storage تنظیم نشده است.");
        }

        var extension = Path.GetExtension(fileName);

        if (string.IsNullOrWhiteSpace(extension))
        {
            throw new ArgumentException(
                "فایل باید دارای پسوند معتبر باشد.",
                nameof(fileName));
        }

        var storageKey =
            $"{folder.Trim('/')}/{Guid.NewGuid():N}{extension}";

        var rootPath = Path.GetFullPath(
            _storageSettings.RootPath);

        var fullPath = Path.Combine(
            rootPath,
            storageKey.Replace(
                '/',
                Path.DirectorySeparatorChar));

        var directory = Path.GetDirectoryName(fullPath);

        if (directory is null)
        {
            throw new InvalidOperationException(
                "مسیر ذخیره‌سازی فایل معتبر نیست.");
        }

        Directory.CreateDirectory(directory);

        await using var outputStream = new FileStream(
            fullPath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 81920,
            useAsync: true);

        await fileStream.CopyToAsync(outputStream);

        return storageKey;
    }

    public Task DeleteAsync(string storageKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(storageKey);

        var fullPath = GetSafeFullPath(storageKey);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string storageKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(storageKey);

        var fullPath = GetSafeFullPath(storageKey);

        return Task.FromResult(
            File.Exists(fullPath));
    }

    private string GetSafeFullPath(string storageKey)
    {
        var rootPath = Path.GetFullPath(
            _storageSettings.RootPath);

        var normalizedKey = storageKey
            .Replace(
                '/',
                Path.DirectorySeparatorChar)
            .Replace(
                '\\',
                Path.DirectorySeparatorChar);

        var fullPath = Path.GetFullPath(
            Path.Combine(rootPath, normalizedKey));

        var rootWithSeparator =
            rootPath.TrimEnd(
                Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;

        if (!fullPath.StartsWith(
                rootWithSeparator,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "مسیر فایل خارج از محدوده Storage مجاز است.");
        }

        return fullPath;
    }
}