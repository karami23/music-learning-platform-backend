using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Articles;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Articles;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Articles;

public class ArticleRepository : IArticleRepository
{
    private readonly MusicEducationDbContext _context;

    public ArticleRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<Article?> GetByIdAsync(int id)
    {
        return await _context.Set<Article>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Article?> GetBySlugAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return null;

        return await _context.Set<Article>()
            .FirstOrDefaultAsync(x => x.Slug == slug);
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        return await _context.Set<Article>()
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetPublishedAsync()
    {
        return await _context.Set<Article>()
            .Where(x => x.Status == ArticleStatus.Published)
            .OrderByDescending(x => x.PublishedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetByCategoryIdAsync(
        int categoryId)
    {
        return await _context.Set<Article>()
            .Where(x => x.ArticleCategoryId == categoryId)
            .ToListAsync();
    }

    public async Task<Article> AddAsync(Article article)
    {
        ArgumentNullException.ThrowIfNull(article);

        await _context.Set<Article>()
            .AddAsync(article);

        return article;
    }

    public Task UpdateAsync(Article article)
    {
        ArgumentNullException.ThrowIfNull(article);

        _context.Set<Article>()
            .Update(article);

        return Task.CompletedTask;
    }
}