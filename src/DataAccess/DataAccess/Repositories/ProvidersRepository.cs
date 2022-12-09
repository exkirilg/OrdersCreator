using Domain.DataAccessInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ProvidersRepository<TDataContext> : GenericRepository<Provider, TDataContext>, IProvidersRepository
    where TDataContext : DbContext, IDataContext
{
    public ProvidersRepository(TDataContext context) : base(context)
    {
    }

    public async Task<int> GetProvidersNumber()
    {
        return await _dbSet.CountAsync();
    }
}
