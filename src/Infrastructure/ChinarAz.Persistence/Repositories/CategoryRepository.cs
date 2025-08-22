using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Domain.Entities;
using ChinarAz.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChinarAz.Persistence.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly ChinarAzDbContext _context;
    public CategoryRepository(ChinarAzDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<List<Category>> GetByNameSearchAsync(string namePart)
    {
        return await _context.Categories
            .Where(c => c.Name.Contains(namePart))
            .ToListAsync();
    }
}
