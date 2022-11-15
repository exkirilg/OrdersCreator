using Domain.DataAccess;
using Domain.Models;

namespace DataAccess.Repositories;

public class ProvidersRepository : GenericRepository<Provider>, IProvidersRepository
{
    public ProvidersRepository(DataContext context) : base(context)
    {
    }
}
