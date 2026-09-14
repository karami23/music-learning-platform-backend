namespace MusicEducation.Application.Commands.Articles.ArticleCategory;

public record UpdateArticleCategoryCommand(
    int Id,
    string Name,
    string? Description
);