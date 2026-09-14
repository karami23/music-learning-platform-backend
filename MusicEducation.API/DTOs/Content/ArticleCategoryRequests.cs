namespace MusicEducation.API.DTOs.Articles.ArticleCategory;

public sealed record CreateArticleCategoryRequest(
    string Name,
    string? Description);

public sealed record UpdateArticleCategoryRequest(
    string Name,
    string? Description);