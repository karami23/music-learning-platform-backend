using MusicEducation.Domain.Constants;
using MusicEducation.Domain.Exceptions;

namespace MusicEducation.Domain.Entities.Courses;

public class CourseCategory : BaseEntity
{
    public string Name { get; private set; } = null!;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    private CourseCategory() { }
    public static CourseCategory Create(
        string name,
        string? description = null)
    {
        return new CourseCategory
        {
            Name = NormalizeName(name),
            Description = NormalizeDescription(description),
            IsActive = true
        };
    }

    public void Update(
        string name,
        string? description)
    {
        Name = NormalizeName(name);
        Description = NormalizeDescription(description);

        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("نام دسته‌بندی الزامی است", nameof(name));

        var normalizedName = name.Trim();

        if (normalizedName.Length > DomainConstants.MaxCourseCategoryNameLength)
            throw new DomainException(
                $"نام دسته‌بندی نمی‌تواند بیشتر از {DomainConstants.MaxCourseCategoryNameLength} کاراکتر باشد",
                nameof(name));

        return normalizedName;
    }

    private static string? NormalizeDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return null;

        var normalizedDescription = description.Trim();

        if (normalizedDescription.Length > DomainConstants.MaxCourseCategoryDescriptionLength)
            throw new DomainException(
                $"توضیحات دسته‌بندی نمی‌تواند بیشتر از {DomainConstants.MaxCourseCategoryDescriptionLength} کاراکتر باشد",
                nameof(description));

        return normalizedDescription;
    }
}