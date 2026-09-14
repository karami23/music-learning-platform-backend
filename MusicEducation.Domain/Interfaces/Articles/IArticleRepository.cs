using MusicEducation.Domain.Entities.Articles;

namespace MusicEducation.Domain.Interfaces.Articles;

public interface IArticleRepository
{
    Task<Article?> GetByIdAsync(int id);

    Task<Article?> GetBySlugAsync(string slug);

    Task<IEnumerable<Article>> GetAllAsync();

    Task<IEnumerable<Article>> GetPublishedAsync();

    Task<IEnumerable<Article>> GetByCategoryIdAsync(int categoryId);

    Task<Article> AddAsync(Article article);

    Task UpdateAsync(Article article);
}