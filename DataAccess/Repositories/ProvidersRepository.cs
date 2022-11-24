using Domain.DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class ProvidersRepository : GenericRepository<Provider>, IProvidersRepository
{
    public ProvidersRepository(DataContext context) : base(context)
    {
    }

    public async Task<int> GetProvidersNumber()
    {
        return await _dbSet.CountAsync();
    }
}
