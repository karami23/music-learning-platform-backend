using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Articles;

public class Article : BaseEntity
{
    public int ArticleCategoryId { get; private set; }

    public string Title { get; private set; } = null!;

    public string Slug { get; private set; } = null!;

    public string? Summary { get; private set; }

    public string Content { get; private set; } = null!;

    public string? CoverImageUrl { get; private set; }

    public string? MetaTitle { get; private set; }

    public string? MetaDescription { get; private set; }

    public ArticleStatus Status { get; private set; }

    public DateTime? PublishedAt { get; private set; }

    private Article()
    {
    }

    public static Article Create(
        int articleCategoryId,
        string title,
        string slug,
        string? summary,
        string content,
        string? coverImageUrl,
        string? metaTitle,
        string? metaDescription)
    {
        ValidateArticleCategoryId(articleCategoryId);

        return new Article
        {
            ArticleCategoryId = articleCategoryId,
            Title = NormalizeTitle(title),
            Slug = NormalizeSlug(slug),
            Summary = NormalizeSummary(summary),
            Content = NormalizeContent(content),
            CoverImageUrl = NormalizeCoverImageUrl(coverImageUrl),
            MetaTitle = NormalizeMetaTitle(metaTitle),
            MetaDescription = NormalizeMetaDescription(metaDescription),
            Status = ArticleStatus.Draft,
            PublishedAt = null
        };
    }

    public void Update(
        int articleCategoryId,
        string title,
        string slug,
        string? summary,
        string content,
        string? coverImageUrl,
        string? metaTitle,
        string? metaDescription)
    {
        ValidateArticleCategoryId(articleCategoryId);

        ArticleCategoryId = articleCategoryId;
        Title = NormalizeTitle(title);
        Slug = NormalizeSlug(slug);
        Summary = NormalizeSummary(summary);
        Content = NormalizeContent(content);
        CoverImageUrl = NormalizeCoverImageUrl(coverImageUrl);
        MetaTitle = NormalizeMetaTitle(metaTitle);
        MetaDescription = NormalizeMetaDescription(metaDescription);

        UpdatedAt = DateTime.UtcNow;
    }

    public void Publish()
    {
        if (Status == ArticleStatus.Published)
            return;

        Status = ArticleStatus.Published;
        PublishedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Unpublish()
    {
        if (Status != ArticleStatus.Published)
            return;

        Status = ArticleStatus.Draft;
        PublishedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Archive()
    {
        if (Status == ArticleStatus.Archived)
            return;

        Status = ArticleStatus.Archived;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateArticleCategoryId(int articleCategoryId)
    {
        if (articleCategoryId <= 0)
            throw new DomainException("شناسه دسته‌بندی مقاله معتبر نیست", nameof(articleCategoryId));
    }

    private static string NormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("عنوان مقاله الزامی است", nameof(title));

        var normalized = title.Trim();

        if (normalized.Length > DomainConstants.MaxArticleTitleLength)
            throw new DomainException(
                $"عنوان مقاله نمی‌تواند بیشتر از {DomainConstants.MaxArticleTitleLength} کاراکتر باشد",
                nameof(title));

        return normalized;
    }

    private static string NormalizeSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new DomainException("اسلاگ مقاله الزامی است", nameof(slug));

        var normalized = slug.Trim().ToLowerInvariant();

        if (normalized.Length > DomainConstants.MaxArticleSlugLength)
            throw new DomainException(
                $"اسلاگ مقاله نمی‌تواند بیشتر از {DomainConstants.MaxArticleSlugLength} کاراکتر باشد",
                nameof(slug));

        return normalized;
    }

    private static string? NormalizeSummary(string? summary)
    {
        if (string.IsNullOrWhiteSpace(summary))
            return null;

        var normalized = summary.Trim();

        if (normalized.Length > DomainConstants.MaxArticleSummaryLength)
            throw new DomainException(
                $"خلاصه مقاله نمی‌تواند بیشتر از {DomainConstants.MaxArticleSummaryLength} کاراکتر باشد",
                nameof(summary));

        return normalized;
    }

    private static string NormalizeContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("محتوای مقاله الزامی است", nameof(content));

        return content.Trim();
    }

    private static string? NormalizeCoverImageUrl(string? coverImageUrl)
    {
        if (string.IsNullOrWhiteSpace(coverImageUrl))
            return null;

        var normalized = coverImageUrl.Trim();

        if (!Uri.TryCreate(
                normalized,
                UriKind.Absolute,
                out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp &&
             uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new DomainException("آدرس تصویر مقاله معتبر نیست", nameof(coverImageUrl));
        }

        if (normalized.Length > DomainConstants.MaxArticleCoverImageUrlLength)
            throw new DomainException(
                $"آدرس تصویر مقاله نمی‌تواند بیشتر از {DomainConstants.MaxArticleCoverImageUrlLength} کاراکتر باشد",
                nameof(coverImageUrl));

        return normalized;
    }

    private static string? NormalizeMetaTitle(string? metaTitle)
    {
        if (string.IsNullOrWhiteSpace(metaTitle))
            return null;

        var normalized = metaTitle.Trim();

        if (normalized.Length > DomainConstants.MaxArticleMetaTitleLength)
            throw new DomainException(
                $"عنوان SEO نمی‌تواند بیشتر از {DomainConstants.MaxArticleMetaTitleLength} کاراکتر باشد",
                nameof(metaTitle));

        return normalized;
    }

    private static string? NormalizeMetaDescription(string? metaDescription)
    {
        if (string.IsNullOrWhiteSpace(metaDescription))
            return null;

        var normalized = metaDescription.Trim();

        if (normalized.Length > DomainConstants.MaxArticleMetaDescriptionLength)
            throw new DomainException(
                $"توضیحات SEO نمی‌تواند بیشتر از {DomainConstants.MaxArticleMetaDescriptionLength} کاراکتر باشد",
                nameof(metaDescription));

        return normalized;
    }
}