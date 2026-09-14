using MusicEducation.Domain.Entities.Identity;

namespace MusicEducation.Domain.Interfaces.Identity;

public interface ITeacherRepository
{
    Task<Teacher?> GetByIdAsync(int id);

    Task<Teacher?> GetByUserIdAsync(int userId);

    Task<IEnumerable<Teacher>> GetAllAsync();

    Task<IEnumerable<Teacher>> GetActiveTeachersAsync();

    Task<Teacher> AddAsync(Teacher teacher);

    Task UpdateAsync(Teacher teacher);

    Task<bool> ExistsByUserIdAsync(int userId);
}