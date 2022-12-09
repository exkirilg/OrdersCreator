using Domain.Models;

namespace Domain.DataAccessInterfaces;

public interface IProvidersRepository : IGenericRepository<Provider>
{
    Task<int> GetProvidersNumber();
}
