using ChinarAz.Domain.Entities;

namespace ChinarAz.Application.Abstracts.Repositories;

public interface IFavouriteRepository : IRepository<Favourite>
{
    Task<List<Favourite>> GetByUserIdAsync(string userId);
    Task<Favourite?> GetByUserAndProductAsync(string userId, Guid productId);
}
