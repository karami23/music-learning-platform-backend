namespace MusicEducation.API.DTOs.Articles.Article;

public sealed record CreateArticleRequest(
    int ArticleCategoryId,
    string Title,
    string Slug,
    string? Summary,
    string Content,
    string? CoverImageUrl,
    string? MetaTitle,
    string? MetaDescription);

public sealed record UpdateArticleRequest(
    int ArticleCategoryId,
    string Title,
    string Slug,
    string? Summary,
    string Content,
    string? CoverImageUrl,
    string? MetaTitle,
    string? MetaDescription);