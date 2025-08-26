using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Domain.Entities;
using ChinarAz.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChinarAz.Persistence.Repositories;

public class BlogRepository : Repository<Blog>, IBlogRepository
{
    private readonly ChinarAzDbContext _context;
    public BlogRepository(ChinarAzDbContext context) : base(context) 
    {
        _context = context;
    }

    public async Task<Blog?> GetWithImagesByIdAsync(Guid id)
    {
        return await _context.Set<Blog>()
            .Include(b => b.Images)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Blog>> GetAllWithImagesAsync()
    {
        return await _context.Set<Blog>()
            .Include(b => b.Images)
            .ToListAsync();
    }
}
