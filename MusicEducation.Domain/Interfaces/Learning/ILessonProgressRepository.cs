using MusicEducation.Domain.Entities.Learning;

namespace MusicEducation.Domain.Interfaces.Learning;

public interface ILessonProgressRepository
{
    Task<LessonProgress?> GetByIdAsync(int id);

    Task<LessonProgress?> GetByUserIdAndLessonIdAsync(int userId, int lessonId);

    Task<IEnumerable<LessonProgress>> GetByUserIdAsync(int userId);

    Task<IEnumerable<LessonProgress>> GetByLessonIdAsync(int lessonId);

    Task<LessonProgress?> GetLastProgressAsync(int userId, int courseId);

    Task<LessonProgress> AddAsync(LessonProgress progress);

    Task UpdateAsync(LessonProgress progress);
}