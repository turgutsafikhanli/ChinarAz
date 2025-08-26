using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Domain.Entities;
using ChinarAz.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChinarAz.Persistence.Repositories;

public class FavouriteRepository : Repository<Favourite>, IFavouriteRepository
{
    public FavouriteRepository(ChinarAzDbContext context) : base(context) { }

    public async Task<List<Favourite>> GetByUserIdAsync(string userId)
    {
        return await GetAllFiltered(f => f.UserId == userId, include: new Expression<Func<Favourite, object>>[] { f => f.Product }).ToListAsync();
    }

    public async Task<Favourite?> GetByUserAndProductAsync(string userId, Guid productId)
    {
        return await GetAllFiltered(f => f.UserId == userId && f.ProductId == productId).FirstOrDefaultAsync();
    }
}
