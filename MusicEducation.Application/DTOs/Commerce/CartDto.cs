namespace MusicEducation.Application.DTOs.Commerce;

public record CartDto(
    int Id,
    int UserId,
    IReadOnlyCollection<CartItemDto> Items
);