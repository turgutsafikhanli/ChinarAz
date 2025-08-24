using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Domain.Entities;
using ChinarAz.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChinarAz.Persistence.Repositories;

public class OrderProductRepository : Repository<OrderProduct>, IOrderProductRepository
{
    private readonly ChinarAzDbContext _context;

    public OrderProductRepository(ChinarAzDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<OrderProduct>> GetByOrderIdAsync(Guid orderId)
    {
        return await _context.OrderProducts
            .Include(op => op.Product)
            .Where(op => op.OrderId == orderId)
            .ToListAsync();
    }
}
