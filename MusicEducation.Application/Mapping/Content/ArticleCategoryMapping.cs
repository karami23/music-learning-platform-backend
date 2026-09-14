using MusicEducation.Application.DTOs.Articles;
using MusicEducation.Domain.Entities.Articles;

namespace MusicEducation.Application.Mappings.Articles;

public static class ArticleCategoryMapping
{
    public static ArticleCategoryDto ToDto(this ArticleCategory category)
    {
        return new ArticleCategoryDto(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive
        );
    }
}