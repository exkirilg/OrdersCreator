using DataAccess.Repositories;
using Domain.DataAccessInterfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class UnitOfWork<TDataContext> : IUnitOfWork where TDataContext : DbContext, IDataContext
{
    private readonly TDataContext _context;

    private bool _disposed = false;

    private IOrdersRepository? _ordersRepository;
    private IProvidersRepository? _providersRepository;

    public IOrdersRepository OrdersRepository
    {
        get
        {
            _ordersRepository ??= new OrdersRepository<TDataContext>(_context);
            return _ordersRepository;
        }
    }

    public IProvidersRepository ProvidersRepository
    {
        get
        {
            _providersRepository ??= new ProvidersRepository<TDataContext>(_context);
            return _providersRepository;
        }
    }

    public UnitOfWork(TDataContext context)
    {
        _context = context;
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed == false)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
