using ChinarAz.Domain.Entities;

namespace ChinarAz.Application.Abstracts.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetByNameSearchAsync(string namePart);
}
