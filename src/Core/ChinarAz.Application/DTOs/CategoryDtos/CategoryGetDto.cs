namespace ChinarAz.Application.DTOs.CategoryDtos;

public record class CategoryGetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}
