using Microsoft.EntityFrameworkCore;
using MusicEducation.Domain.Entities.Reviews;
using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Interfaces.Reviews;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Reviews;

public class ReviewRepository : IReviewRepository
{
    private readonly MusicEducationDbContext _context;

    public ReviewRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<Review?> GetByIdAsync(int id)
    {
        return await _context.Set<Review>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Review>> GetByCourseIdAsync(int courseId)
    {
        return await _context.Set<Review>()
            .Where(x => x.CourseId == courseId)
            .OrderByDescending(x => x.ReviewedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByUserIdAsync(int userId)
    {
        return await _context.Set<Review>()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.ReviewedAt)
            .ToListAsync();
    }

    public async Task<Review?> GetByUserIdAndCourseIdAsync(
        int userId,
        int courseId)
    {
        return await _context.Set<Review>()
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.CourseId == courseId);
    }

    public async Task<IEnumerable<Review>> GetApprovedByCourseIdAsync(
        int courseId)
    {
        return await _context.Set<Review>()
            .Where(x =>
                x.CourseId == courseId &&
                x.Status == ReviewStatus.Approved)
            .OrderByDescending(x => x.ReviewedAt)
            .ToListAsync();
    }

    public async Task<bool> HasReviewedAsync(int userId, int courseId)
    {
        return await _context.Set<Review>()
            .AnyAsync(x =>
                x.UserId == userId &&
                x.CourseId == courseId);
    }

    public async Task<Review> AddAsync(Review review)
    {
        ArgumentNullException.ThrowIfNull(review);

        await _context.Set<Review>()
            .AddAsync(review);

        return review;
    }

    public Task UpdateAsync(Review review)
    {
        ArgumentNullException.ThrowIfNull(review);

        _context.Set<Review>()
            .Update(review);

        return Task.CompletedTask;
    }
}