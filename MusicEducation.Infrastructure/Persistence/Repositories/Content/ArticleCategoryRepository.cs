using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Articles;
using MusicEducation.Domain.Interfaces.Articles;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Articles;

public class ArticleCategoryRepository : IArticleCategoryRepository
{
    private readonly MusicEducationDbContext _context;

    public ArticleCategoryRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<ArticleCategory?> GetByIdAsync(int id)
    {
        return await _context.Set<ArticleCategory>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<ArticleCategory>> GetAllAsync()
    {
        return await _context.Set<ArticleCategory>()
            .ToListAsync();
    }

    public async Task<IEnumerable<ArticleCategory>> GetActiveAsync()
    {
        return await _context.Set<ArticleCategory>()
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task<ArticleCategory> AddAsync(
        ArticleCategory category)
    {
        ArgumentNullException.ThrowIfNull(category);

        await _context.Set<ArticleCategory>()
            .AddAsync(category);

        return category;
    }

    public Task UpdateAsync(ArticleCategory category)
    {
        ArgumentNullException.ThrowIfNull(category);

        _context.Set<ArticleCategory>()
            .Update(category);

        return Task.CompletedTask;
    }
}