namespace ChinarAz.Application.DTOs.OrderProductDtos;

public record class OrderProductCreateDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; } = 0;       // IsWeighted=false üçün
    public decimal WeightGram { get; set; } = 0; // IsWeighted=true üçün
}
