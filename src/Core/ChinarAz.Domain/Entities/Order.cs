using ChinarAz.Domain.Enums;

namespace ChinarAz.Domain.Entities;

public class Order : BaseEntity
{
    public string UserId { get; set; } = null!;
    public AppUser User { get; set; } = null!;
    public decimal TotalPrice { get; set; } = 0;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
