using DataAccess.Repositories;
using Domain.DataAccess;

namespace DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    private bool _disposed = false;

    private IOrdersRepository? _ordersRepository;
    private IProvidersRepository? _providersRepository;

    public IOrdersRepository OrdersRepository
    {
        get
        {
            if (_ordersRepository is null)
            {
                _ordersRepository = new OrdersRepository(_context);
            }
            return _ordersRepository;
        }
    }

    public IProvidersRepository ProvidersRepository
    {
        get
        {
            if (_providersRepository is null)
            {
                _providersRepository = new ProvidersRepository(_context);
            }
            return _providersRepository;
        }
    }

    public UnitOfWork(DataContext context)
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
