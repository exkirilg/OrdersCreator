using Domain.Models;

namespace Domain.DataAccess;

public interface IProvidersRepository : IGenericRepository<Provider>
{
    Task<int> GetProvidersNumber();
}
