using ChinarAz.Domain.Entities;

namespace ChinarAz.Application.Abstracts.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
}
