namespace MusicEducation.Application.Commands.Articles.ArticleCategory;

public record CreateArticleCategoryCommand(
    string Name,
    string? Description
);