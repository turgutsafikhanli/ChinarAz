using ChinarAz.Domain.Entities;

namespace ChinarAz.Application.Abstracts.Repositories;

public interface IOrderProductRepository : IRepository<OrderProduct>
{
    Task<List<OrderProduct>> GetByOrderIdAsync(Guid orderId);
}
