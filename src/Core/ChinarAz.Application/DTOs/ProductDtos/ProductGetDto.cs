namespace ChinarAz.Application.DTOs.ProductDtos;

public record class ProductGetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public bool IsWeighted { get; set; }
    public decimal Price { get; set; }
    public decimal WeightGram { get; set; } = 0;
}
