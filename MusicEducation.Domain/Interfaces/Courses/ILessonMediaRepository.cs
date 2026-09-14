using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Enums;

namespace MusicEducation.Domain.Interfaces.Courses;

public interface ILessonMediaRepository
{
    Task<LessonMedia?> GetByIdAsync(int id);

    Task<IEnumerable<LessonMedia>> GetAllAsync();

    Task<IEnumerable<LessonMedia>> GetByLessonIdAsync(int lessonId);

    Task<IEnumerable<LessonMedia>> GetByLessonIdAndTypeAsync(
        int lessonId,
        LessonMediaType mediaType);

    Task<LessonMedia?> GetByLessonIdAndOrderAsync(
        int lessonId,
        int order);

    Task<LessonMedia?> GetFirstMediaAsync(int lessonId);

    Task<LessonMedia?> GetNextMediaAsync(int mediaId);

    Task<LessonMedia> AddAsync(LessonMedia media);

    Task UpdateAsync(LessonMedia media);
}