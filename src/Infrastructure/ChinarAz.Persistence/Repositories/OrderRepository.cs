using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Domain.Entities;
using ChinarAz.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChinarAz.Persistence.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly ChinarAzDbContext _context;
    public OrderRepository(ChinarAzDbContext context) : base(context) 
    {
        _context = context;
    }

    public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId.ToString())
            .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
            .AsNoTracking()
            .ToListAsync();
    }
}
