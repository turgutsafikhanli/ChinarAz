using ChinarAz.Application.Abstracts.Repositories;
using ChinarAz.Domain.Entities;
using ChinarAz.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChinarAz.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity, new()
{
    private ChinarAzDbContext _context { get; }
    private DbSet<T> Table { get; }

    public Repository(ChinarAzDbContext context)
    {
        _context = context;
        Table = _context.Set<T>();

    }

    public async Task AddAsync(T entity)
    {
        await Table.AddAsync(entity);
    }

    public void Update(T entity)
    {
        Table.Update(entity);
    }

    public void Delete(T entity)
    {
        Table.Remove(entity);
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await Table.FindAsync(id);
    }

    public IQueryable<T> GetByFiltered(Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? include = null,
        bool isTracking = false)
    {
        IQueryable<T> query = Table;


        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (include != null)
        {
            foreach (var includeExpression in include)
            {
                query = query.Include(includeExpression);
            }
        }

        if (!isTracking)
        {
            query = query.AsNoTracking(); // Return the table without tracking changes
        }
        return query;
    }

    public IQueryable<T> GetAll(bool isTracking = false)
    {
        if (!isTracking)
            return Table.AsNoTracking(); // Return the table without tracking changes
        return Table;
    }

    public IQueryable<T> GetAllFiltered(Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>[]? include = null,
        Expression<Func<T, bool>>? orderBy = null,
        bool isOrderByAsc = true,
        bool isTracking = false)
    {
        IQueryable<T> query = Table;
        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (include != null)
        {
            foreach (var includeExpression in include)
            {
                query = query.Include(includeExpression);
            }
        }

        if (orderBy != null)
        {
            query = isOrderByAsc ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }

        if (!isTracking)
        {
            query = query.AsNoTracking(); // Return the table without tracking changes
        }
        return query;
    }

    public async Task SaveChangeAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(T entity)
    {
        entity.IsDeleted = true;
        Table.Update(entity);
        await _context.SaveChangesAsync();
    }
}
