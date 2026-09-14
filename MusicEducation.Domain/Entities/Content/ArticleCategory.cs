using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Articles;

public class ArticleCategory : BaseEntity
{
    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    private readonly List<Article> _articles = new();

    public IReadOnlyCollection<Article> Articles => _articles.AsReadOnly();

    private ArticleCategory() { }

    public static ArticleCategory Create(
        string name,
        string? description)
    {
        var normalizedName = NormalizeName(name);

        return new ArticleCategory
        {
            Name = normalizedName,
            Description = NormalizeDescription(description),
            IsActive = true
        };
    }

    public void Update(
        string name,
        string? description)
    {
        Name = NormalizeName(name);
        Description = NormalizeDescription(description);

        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("نام دسته‌بندی مقاله الزامی است", nameof(name));

        var normalized = name.Trim();

        if (normalized.Length > DomainConstants.MaxArticleCategoryNameLength)
            throw new DomainException(
                $"نام دسته‌بندی نمی‌تواند بیشتر از {DomainConstants.MaxArticleCategoryNameLength} کاراکتر باشد",
                nameof(name));

        return normalized;
    }

    private static string? NormalizeDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return null;

        var normalized = description.Trim();

        if (normalized.Length > DomainConstants.MaxArticleCategoryDescriptionLength)
            throw new DomainException(
                $"توضیحات دسته‌بندی نمی‌تواند بیشتر از {DomainConstants.MaxArticleCategoryDescriptionLength} کاراکتر باشد",
                nameof(description));

        return normalized;
    }
}