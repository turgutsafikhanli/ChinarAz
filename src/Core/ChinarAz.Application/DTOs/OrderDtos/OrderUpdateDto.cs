using ChinarAz.Application.DTOs.OrderProductDtos;
using ChinarAz.Domain.Enums;

namespace ChinarAz.Application.DTOs.OrderDtos;

public record class OrderUpdateDto
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
}
