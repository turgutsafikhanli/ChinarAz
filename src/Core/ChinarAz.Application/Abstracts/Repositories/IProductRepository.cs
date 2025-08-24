using ChinarAz.Domain.Entities;

namespace ChinarAz.Application.Abstracts.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetByCategoryIdAsync(Guid categoryId); // Müştəri üçün
    Task<List<Product>> GetByNameAsync(string name); // Admin
    Task<List<Product>> SearchAsync(string keyword); // Müştəri
}
