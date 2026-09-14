namespace MusicEducation.Application.DTOs.Commerce;

public record CartItemDto(
    int Id,
    int CartId,
    int CourseId
);