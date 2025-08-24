using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Domain.Entities;
using ChinarAz.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChinarAz.Persistence.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ChinarAzDbContext context) : base(context) { }

    public async Task<List<Product>> GetByCategoryIdAsync(Guid categoryId)
    {
        return await GetByFiltered(p => p.CategoryId == categoryId).ToListAsync();
    }

    public async Task<List<Product>> GetByNameAsync(string name)
    {
        return await GetByFiltered(p => p.Name.Contains(name)).ToListAsync();
    }

    public async Task<List<Product>> SearchAsync(string keyword)
    {
        return await GetByFiltered(p => p.Name.Contains(keyword)).ToListAsync();
    }
}
