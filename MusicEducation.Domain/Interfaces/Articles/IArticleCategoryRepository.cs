using MusicEducation.Domain.Entities.Articles;

namespace MusicEducation.Domain.Interfaces.Articles;

public interface IArticleCategoryRepository
{
    Task<ArticleCategory?> GetByIdAsync(int id);

    Task<IEnumerable<ArticleCategory>> GetAllAsync();

    Task<IEnumerable<ArticleCategory>> GetActiveAsync();

    Task<ArticleCategory> AddAsync(ArticleCategory category);

    Task UpdateAsync(ArticleCategory category);
}