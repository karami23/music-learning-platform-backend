using Microsoft.EntityFrameworkCore;
using MusicEducation.Application.Interfaces.Courses;
using MusicEducation.Domain.Entities.Courses;
using MusicEducation.Domain.Entities.Identity;
using MusicEducation.Infrastructure.Persistence.DbContext;

namespace MusicEducation.Infrastructure.Repositories.Courses;

public class CourseReadRepository : ICourseReadRepository
{
    private readonly MusicEducationDbContext _context;

    public CourseReadRepository(MusicEducationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CourseListReadModel>> GetPagedAsync(
        string? search,
        int? teacherId,
        int? courseCategoryId,
        int page,
        int pageSize)
    {
        var query =
            from course in _context.Set<Course>().AsNoTracking()
            join teacher in _context.Set<Teacher>()
                on course.TeacherId equals teacher.UserId
            join user in _context.Set<User>()
                on teacher.UserId equals user.Id
            join category in _context.Set<CourseCategory>()
                on course.CourseCategoryId equals category.Id
            where course.Status == CourseStatus.Published
            select new
            {
                Course = course,
                Teacher = teacher,
                User = user,
                Category = category
            };

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();

            query = query.Where(x =>
                x.Course.Title.Contains(normalizedSearch) ||
                x.Course.Description.Contains(normalizedSearch));
        }

        if (teacherId.HasValue)
        {
            query = query.Where(x =>
                x.Course.TeacherId == teacherId.Value);
        }

        if (courseCategoryId.HasValue)
        {
            query = query.Where(x =>
                x.Course.CourseCategoryId == courseCategoryId.Value);
        }

        return await query
            .OrderByDescending(x => x.Course.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new CourseListReadModel(
                x.Course.Id,
                x.Course.Title,
                x.Course.Slug,
                x.Course.Price != null
                    ? x.Course.Price.Amount
                    : null,
                x.Course.Price != null
                    ? x.Course.Price.Currency
                    : null,
                x.Course.PricingType,
                x.Course.CoverImageUrl,
                x.Course.TeacherId,
                x.User.FirstName + " " + x.User.LastName,
                x.Course.CourseCategoryId,
                x.Category.Name
            ))
            .ToListAsync();
    }

    public async Task<CourseDetailsReadModel?> GetBySlugAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return null;

        var normalizedSlug = slug.Trim().ToLowerInvariant();

        return await (
            from course in _context.Set<Course>().AsNoTracking()
            join teacher in _context.Set<Teacher>()
                on course.TeacherId equals teacher.UserId
            join user in _context.Set<User>()
                on teacher.UserId equals user.Id
            join category in _context.Set<CourseCategory>()
                on course.CourseCategoryId equals category.Id
            where course.Slug == normalizedSlug
            select new CourseDetailsReadModel(
                course.Id,
                course.Title,
                course.Slug,
                course.Description,
                course.Price != null
                    ? course.Price.Amount
                    : null,
                course.Price != null
                    ? course.Price.Currency
                    : null,
                course.PricingType,
                course.Status,
                course.CoverImageUrl,
                course.TeacherId,
                user.FirstName + " " + user.LastName,
                teacher.Bio,
                course.CourseCategoryId,
                category.Name
            )
        ).FirstOrDefaultAsync();
    }

    public async Task<CourseDetailsReadModel?> GetByIdAsync(int id)
    {
        return await (
            from course in _context.Set<Course>().AsNoTracking()
            join teacher in _context.Set<Teacher>()
                on course.TeacherId equals teacher.UserId
            join user in _context.Set<User>()
                on teacher.UserId equals user.Id
            join category in _context.Set<CourseCategory>()
                on course.CourseCategoryId equals category.Id
            where course.Id == id
            select new CourseDetailsReadModel(
                course.Id,
                course.Title,
                course.Slug,
                course.Description,
                course.Price != null
                    ? course.Price.Amount
                    : null,
                course.Price != null
                    ? course.Price.Currency
                    : null,
                course.PricingType,
                course.Status,
                course.CoverImageUrl,
                course.TeacherId,
                user.FirstName + " " + user.LastName,
                teacher.Bio,
                course.CourseCategoryId,
                category.Name
            )
        ).FirstOrDefaultAsync();
    }
}