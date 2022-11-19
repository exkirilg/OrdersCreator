using Domain.DataAccess;
using Domain.DTO;
using Domain.Models;
using Domain.Services;

namespace Services;

public class ProvidersServices : IProvidersServices
{
    private readonly IUnitOfWork _unitOfWork;

    public ProvidersServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Provider>> GetAll()
    {
        return await _unitOfWork.ProvidersRepository
            .Get(orderBy: q => q.OrderBy(p => p.Name));
    }

    public async Task<Provider> GetById(int id)
    {
        return await _unitOfWork.ProvidersRepository.GetById(id);
    }

    public async Task Create(NewProviderRequest request)
    {
        Provider provider = ConvertToProvider(request);
        await _unitOfWork.ProvidersRepository.Insert(provider);
        await _unitOfWork.Save();
    }

    public async Task Update(UpdateProviderRequest request)
    {
        Provider provider = await GetById(request.Id);
        UpdateProviderByRequest(provider, request);
        _unitOfWork.ProvidersRepository.Update(provider);
        await _unitOfWork.Save();
    }

    public async Task Delete(int id)
    {
        await _unitOfWork.ProvidersRepository.Delete(id);
        await _unitOfWork.Save();
    }

    private Provider ConvertToProvider(NewProviderRequest request)
    {
        return new Provider(request.Name);
    }

    private void UpdateProviderByRequest(Provider provider, UpdateProviderRequest request)
    {
        provider.Name = request.Name;
    }
}
