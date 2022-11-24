using Domain.DTO;
using Domain.Models;

namespace Domain.Services;

public interface IProvidersServices
{
    Task<GetProvidersResponse> Get(GetProvidersRequest request);
    Task<Provider> GetById(int id);
    Task Create(NewProviderRequest request);
    Task Update(UpdateProviderRequest request);
    Task Delete(int id);
}