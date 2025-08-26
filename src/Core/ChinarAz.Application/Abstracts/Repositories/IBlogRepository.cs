using ChinarAz.Domain.Entities;

namespace ChinarAz.Application.Abstracts.Repositories;

public interface IBlogRepository : IRepository<Blog>
{
    Task<Blog?> GetWithImagesByIdAsync(Guid id);
    Task<List<Blog>> GetAllWithImagesAsync();
}
