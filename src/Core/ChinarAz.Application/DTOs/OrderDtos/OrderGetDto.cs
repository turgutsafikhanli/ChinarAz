using ChinarAz.Application.DTOs.OrderProductDtos;
using ChinarAz.Domain.Enums;

namespace ChinarAz.Application.DTOs.OrderDtos;

public record class OrderGetDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderProductGetDto> Products { get; set; } = new();
}
