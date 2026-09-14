using MusicEducation.Domain.Enums;
using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Learning;

public class CourseAccess : BaseEntity
{
    public int UserId { get; private set; }

    public int CourseId { get; private set; }

    public CourseAccessStatus Status { get; private set; }

    public DateTime GrantedAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    private CourseAccess() { }

    public static CourseAccess Create(int userId, int courseId)
    {
        ValidateUserId(userId);
        ValidateCourseId(courseId);

        return new CourseAccess
        {
            UserId = userId,
            CourseId = courseId,
            Status = CourseAccessStatus.Active,
            GrantedAt = DateTime.UtcNow
        };
    }

    public void Revoke()
    {
        if (Status == CourseAccessStatus.Revoked)
            return;

        Status = CourseAccessStatus.Revoked;
        RevokedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        if (Status == CourseAccessStatus.Active)
            return;

        Status = CourseAccessStatus.Active;
        RevokedAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidateUserId(int userId)
    {
        if (userId <= 0)
            throw new DomainException("شناسه کاربر معتبر نیست", nameof(userId));
    }

    private static void ValidateCourseId(int courseId)
    {
        if (courseId <= 0)
            throw new DomainException("شناسه دوره معتبر نیست", nameof(courseId));
    }
}