using MusicEducation.Domain.Enums;

namespace MusicEducation.Application.DTOs.Articles;

public record ArticleDto(
    int Id,
    int ArticleCategoryId,
    string Title,
    string Slug,
    string? Summary,
    string Content,
    string? CoverImageUrl,
    string? MetaTitle,
    string? MetaDescription,
    ArticleStatus Status,
    DateTime? PublishedAt
);