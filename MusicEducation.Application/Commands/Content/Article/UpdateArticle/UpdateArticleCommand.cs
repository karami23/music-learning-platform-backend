namespace MusicEducation.Application.Commands.Articles.Article;

public record UpdateArticleCommand(
    int Id,
    int ArticleCategoryId,
    string Title,
    string Slug,
    string? Summary,
    string Content,
    string? CoverImageUrl,
    string? MetaTitle,
    string? MetaDescription
);