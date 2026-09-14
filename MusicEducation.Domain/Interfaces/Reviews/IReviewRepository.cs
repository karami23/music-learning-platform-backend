using MusicEducation.Domain.Entities.Reviews;

namespace MusicEducation.Domain.Interfaces.Reviews;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(int id);

    Task<IEnumerable<Review>> GetByCourseIdAsync(int courseId);

    Task<IEnumerable<Review>> GetByUserIdAsync(int userId);

    Task<Review?> GetByUserIdAndCourseIdAsync(int userId, int courseId);

    Task<IEnumerable<Review>> GetApprovedByCourseIdAsync( int courseId);

    Task<bool> HasReviewedAsync(int userId, int courseId);

    Task<Review> AddAsync(Review review);

    Task UpdateAsync(Review review);
}