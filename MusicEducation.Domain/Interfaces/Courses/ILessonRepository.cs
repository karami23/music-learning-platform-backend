using MusicEducation.Domain.Entities.Courses;

namespace MusicEducation.Domain.Interfaces.Courses;

public interface ILessonRepository
{
    Task<Lesson?> GetByIdAsync(int id);

    Task<IEnumerable<Lesson>> GetAllAsync();

    Task<IEnumerable<Lesson>> GetByChapterIdAsync(int chapterId);

    Task<Lesson?> GetByChapterIdAndOrderAsync(int chapterId, int order);

    Task<Lesson?> GetFirstLessonAsync(int chapterId);

    Task<Lesson?> GetNextLessonAsync(int lessonId);

    Task<Lesson> AddAsync(Lesson lesson);

    Task UpdateAsync(Lesson lesson);
}