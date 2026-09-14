namespace MusicEducation.Application.DTOs.Articles;

public record ArticleCategoryDto(
    int Id,
    string Name,
    string? Description,
    bool IsActive
);