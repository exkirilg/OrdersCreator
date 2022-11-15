using Domain.DTO;
using Domain.Models;

namespace Domain.Services;

public interface IProvidersServices
{
    Task<IEnumerable<Provider>> GetAll();
    Task<Provider> GetById(int id);
    Task Create(NewProviderRequest request);
    Task Update(UpdateProviderRequest request);
    Task Delete(int id);
}