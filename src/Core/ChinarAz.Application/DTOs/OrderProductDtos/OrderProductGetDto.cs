namespace ChinarAz.Application.DTOs.OrderProductDtos;

public record class OrderProductGetDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal WeightGram { get; set; }
    public decimal UnitPrice { get; set; }
}
