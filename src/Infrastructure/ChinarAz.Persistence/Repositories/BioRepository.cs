using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Domain.Entities;
using ChinarAz.Persistence.Contexts;

namespace ChinarAz.Persistence.Repositories;

public class BioRepository : Repository<Bio>, IBioRepository
{
    private readonly ChinarAzDbContext _context;
    public BioRepository(ChinarAzDbContext context) : base(context)
    {
        _context = context;
    }
}
