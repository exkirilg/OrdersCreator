using Domain.CustomExceptions;
using Domain.DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories;

public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
{
    protected readonly DataContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DataContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        int? limit = null,
        int? offset = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        if (offset is not null)
        {
            query = query.Skip((int) offset);
        }

        if (limit is not null)
        {
            query = query.Take((int)limit);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return await query.ToListAsync();
    }

    public async Task<TEntity> GetById(int id, string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbSet.Where(e => e.Id == id);

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        TEntity? entity = await query.FirstAsync();

        if (entity is null)
        {
            throw new NoEntityFoundByIdException(
                $"There is no {typeof(TEntity).Name.ToLower()} with id {id}");
        }

        return entity;
    }

    public async Task Insert(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public async Task Delete(int id)
    {
        TEntity entity = await GetById(id);

        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }

        _dbSet.Remove(entity);
    }
}
