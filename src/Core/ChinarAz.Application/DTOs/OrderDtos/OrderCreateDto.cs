using ChinarAz.Application.DTOs.OrderProductDtos;

namespace ChinarAz.Application.DTOs.OrderDtos;

public record class OrderCreateDto
{
    public List<OrderProductCreateDto> Products { get; set; } = new();
}
