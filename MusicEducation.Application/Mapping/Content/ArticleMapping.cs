using MusicEducation.Application.DTOs.Articles;
using MusicEducation.Domain.Entities.Articles;

namespace MusicEducation.Application.Mappings.Articles;

public static class ArticleMapping
{
    public static ArticleDto ToDto(this Article article)
    {
        return new ArticleDto(
            article.Id,
            article.ArticleCategoryId,
            article.Title,
            article.Slug,
            article.Summary,
            article.Content,
            article.CoverImageUrl,
            article.MetaTitle,
            article.MetaDescription,
            article.Status,
            article.PublishedAt
        );
    }
}