namespace ChinarAz.Application.DTOs.ProductDtos;

public record class ProductCreateDto
{
    public string Name { get; set; } = null!;
    public Guid CategoryId { get; set; } // Product yaradılarkən category id saxlanır
    public bool IsWeighted { get; set; }
    public decimal Price { get; set; }
    public decimal WeightGram { get; set; } = 0; // IsWeighted true olduqda
}
